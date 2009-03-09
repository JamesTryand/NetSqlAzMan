
#include "stdafx.h"
#include "resource.h"
#include "ShowActiveDirUsers.h"
#include <lmcons.h>
#include <lm.h>
#include <lmserver.h>
#define SECURITY_WIN32 1
#include "Sspi.h" 
#include "security.h"
#include <stack>

#include "ADObjectPicker.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif


CShowActiveDirUsers::CShowActiveDirUsers()
{
	ZeroMemory(&m_stg, sizeof(STGMEDIUM));
}



CShowActiveDirUsers::~CShowActiveDirUsers()
{
	if (m_stg.hGlobal)
	{
		ReleaseStgMedium(&m_stg);	
	}
}



CShowActiveDirUsers::CShowActiveDirUsers(HWND hWnd, CADObjectPicker* pObjPicker)
{
	ZeroMemory(&m_stg, sizeof(STGMEDIUM));
	m_hWnd = hWnd;
	m_pObjPicker = pObjPicker;

}


void CShowActiveDirUsers::Init(HWND hWnd, CADObjectPicker* pObjPicker)
{
	if (m_stg.hGlobal)
	{
		ReleaseStgMedium(&m_stg);	
	}
	m_stg.hGlobal = NULL;
	m_hWnd = hWnd;
	m_pObjPicker = pObjPicker;
}

bool CShowActiveDirUsers::ShowUserSelectionDialog()
{
	_COM_SMARTPTR_TYPEDEF(IDsObjectPicker, IID_IDsObjectPicker);

	IDsObjectPickerPtr		ptrObjPick (CLSID_DsObjectPicker);	
	// semi-smart pointer to object
	static const int		SCOPE_INIT_COUNT = 2;
	DSOP_SCOPE_INIT_INFO	aScopeInit[SCOPE_INIT_COUNT];
	DSOP_INIT_INFO			InitInfo;
	HRESULT					hr = S_OK;
	CComBSTR				strTitle;
	CComBSTR				strMessage;

	ZeroMemory(&InitInfo, sizeof(InitInfo));

	PCWSTR Attributes[] =
	        {
	            L"objectSid",
	        };


	InitInfo.cbSize = sizeof(InitInfo);
	InitInfo.pwzTargetComputer = (LPCOLESTR)m_pObjPicker->m_bstrComputerName; 
	InitInfo.cDsScopeInfos = SCOPE_INIT_COUNT;
	InitInfo.aDsScopeInfos = aScopeInit;
	InitInfo.cAttributesToFetch = 1; //sizeof(Attributes) / sizeof(Attributes[0]);
	InitInfo.apwzAttributeNames = Attributes; 
	InitInfo.flOptions = m_pObjPicker->m_InitInfoFlags;

	ZeroMemory(aScopeInit, 	sizeof(DSOP_SCOPE_INIT_INFO) * SCOPE_INIT_COUNT);

	// Combine multiple scope types in a single array entry.
	aScopeInit[0].cbSize = sizeof(DSOP_SCOPE_INIT_INFO);
	aScopeInit[0].flType = m_pObjPicker->m_ScopeTypeFlags;
	aScopeInit[0].flScope = m_pObjPicker->m_ScopeFlags;
	aScopeInit[0].FilterFlags.Uplevel.flBothModes = m_pObjPicker->m_UplevelFilterFlags_Both;
	aScopeInit[0].FilterFlags.Uplevel.flMixedModeOnly = m_pObjPicker->m_UplevelFilterFlags_Mixed;
	aScopeInit[0].FilterFlags.Uplevel.flNativeModeOnly = m_pObjPicker->m_UplevelFilterFlags_Native;
	aScopeInit[0].FilterFlags.flDownlevel = m_pObjPicker->m_DownlevelFilterFlags;

	aScopeInit[1].cbSize = sizeof(DSOP_SCOPE_INIT_INFO);
	aScopeInit[1].flType = 0x37e;
	aScopeInit[1].flScope = m_pObjPicker->m_ScopeFlags;
	aScopeInit[1].FilterFlags.Uplevel.flBothModes = m_pObjPicker->m_UplevelFilterFlags_Both;
	aScopeInit[1].FilterFlags.Uplevel.flMixedModeOnly = m_pObjPicker->m_UplevelFilterFlags_Mixed;
	aScopeInit[1].FilterFlags.Uplevel.flNativeModeOnly = m_pObjPicker->m_UplevelFilterFlags_Native;
	aScopeInit[1].FilterFlags.flDownlevel = m_pObjPicker->m_DownlevelFilterFlags;

	// bail out if we could not do anything
	if (ptrObjPick == NULL || !IsOSVersionOK())
		return false;


	hr = ptrObjPick->Initialize(&InitInfo);
	if (!SUCCEEDED(hr))
	{
		return false;
	}

	// release memory from possible previous calls
	if (m_stg.hGlobal)
		ReleaseStgMedium(&m_stg);
	m_pDataObject = NULL;

	// make the call to show the dialog that we want
	hr = ptrObjPick->InvokeDialog(m_hWnd, (IDataObject**)&m_pDataObject);

	if (!SUCCEEDED(hr))
	{
		return false;
	}

	if (S_FALSE == hr)
		return false;
	else
		return true;
}


DS_SELECTION_LIST* CShowActiveDirUsers::RetrieveUserSelectionList()
{
	UINT	regClipFormat		= 0;
	HRESULT hr					= S_OK;
	DS_SELECTION_LIST* pSelList = NULL;

	regClipFormat = 
			RegisterClipboardFormat(_T("CFSTR_DSOP_DS_SELECTION_LIST"));
	if ( !regClipFormat )
		return NULL;

	FORMATETC format;
	format.cfFormat = (CLIPFORMAT)regClipFormat;
	format.ptd = NULL;
	format.dwAspect = 
		format.lindex = -1;
	format.tymed = TYMED_HGLOBAL;

	if (m_pDataObject == NULL)
		return NULL;

	hr = m_pDataObject->GetData( &format, &m_stg);


	if ( FAILED(hr) || m_stg.tymed != TYMED_HGLOBAL )
		return NULL;

	pSelList = (DS_SELECTION_LIST*)GlobalLock( m_stg.hGlobal );

	return pSelList;
}


bool CShowActiveDirUsers::IsOSVersionOK()
{
	OSVERSIONINFO info;

	info.dwOSVersionInfoSize = sizeof( OSVERSIONINFO);
	if ( !::GetVersionEx( &info ) )
		return false;

	if ( info.dwMajorVersion < 5 )
		return false;	// before Win 2K

	return true;

}


BOOL CShowActiveDirUsers::GetUserInfoFromActiveDirectorySelection(
				DS_SELECTION& ds, LPTSTR szUser, 
				LPTSTR szDomain, LPTSTR szFullUserName, 
				LPTSTR szDescription)
{
	// handle the AD Native, username will be in ds.pwzUPN in the 
	// form user@domain.com
	if (_tcslen(ds.pwzUPN))
	{
		return GetFullUserName(ds.pwzUPN, szDomain, szFullUserName, 
									szDescription);
	}

	// handle the usual case, ds.pwzADSPath something like 
	// WinNT://WALLYWORLD/MACHINE_NAME/userAccount
	// we parse to get MACHINE_NAME and userAccount
	if (!GetUserAndDomainFromADsPath(ds.pwzADsPath, szUser, szDomain))
	{
		return FALSE;
	}

	// retrieve details about the account
	return GetFullUserName(szUser, szDomain, szFullUserName, szDescription);

}


BOOL CShowActiveDirUsers::GetUserAndDomainFromADsPath(LPTSTR szADsPath, 
										LPTSTR szUser, LPTSTR szDomain)
{
	UINT i=0;
	int iOffsetUser = 0;
	int iOffsetDomain = 0;
	std::stack<int> st;

	if (!_tcslen(szADsPath))
		return FALSE;


	for (i=0; i<_tcslen(szADsPath); i++)
	{
		if (szADsPath[i] == _T('/'))
			st.push(i);
	}

	iOffsetUser = st.top();
	st.pop();
	iOffsetDomain = st.top();

	if (iOffsetUser == 0 || iOffsetDomain == 0 
						|| iOffsetUser < iOffsetDomain)
		return FALSE;

	_tcscpy(szUser, &szADsPath[iOffsetUser + 1]);
	_tcsncpy(szDomain, &szADsPath[iOffsetDomain + 1], 
					iOffsetUser - iOffsetDomain - 1);

	return TRUE;

}



BOOL CShowActiveDirUsers::GetFullUserName(LPTSTR szUser, LPTSTR szDomain, 
							LPTSTR szFullUserName, LPTSTR szDescription)
{
	BOOL	bResult = FALSE;
	LPWSTR	pServer = NULL;
	BOOL	bLocal = FALSE;
	TCHAR	szComputerName[_MAX_PATH];
	TCHAR   szDomainName[_MAX_PATH];
	TCHAR   szServer[_MAX_PATH];
	TCHAR   szPDC[_MAX_PATH];
	DWORD	dwLevel = 10;
	LPUSER_INFO_10 pUserInfoBuffer = NULL;

	// init
	ZeroMemory(szComputerName, _MAX_PATH);
	ZeroMemory(szDomainName, _MAX_PATH);
	ZeroMemory(szServer, _MAX_PATH);
	ZeroMemory(szPDC, _MAX_PATH);
	

	GetLocalComputerName(szComputerName);

	if (_tcsicmp(szComputerName, szDomain) == 0)
		bLocal = TRUE;

	// for active directory native, szUser comes in as user@domain.com
	if (!bLocal) 
	{
		GetDCName(szPDC, szDomain);
		if (_tcschr(szUser, _T('@')) )
		{
			GetUserAndDomainNameFromUPN(szUser, szFullUserName, szDomainName);
			_tcscpy(szUser, szFullUserName);
			_tcscpy(szDomain, szDomainName);
		}
	}
	else
	{
		NetApiBufferAllocate(_MAX_PATH,  (LPVOID*)&pServer);
		ZeroMemory(pServer, _MAX_PATH);
		_tcscpy(pServer, _T("\\\\"));
		_tcscat(pServer, szComputerName);

	}

	// grab the details of the user account, 
	// may fail if AD=native & user not in proper security group
	if (NERR_Success == NetUserGetInfo(szPDC, szUser, dwLevel, 
											(LPBYTE*)&pUserInfoBuffer))
	{
		if (pUserInfoBuffer)
		{
			_tcscpy(szFullUserName, pUserInfoBuffer->usri10_full_name);
			_tcscpy(szDescription, pUserInfoBuffer->usri10_comment);
			bResult = TRUE;
		}
	}

	if (pUserInfoBuffer)
		NetApiBufferFree(pUserInfoBuffer);

	if (pServer)
		NetApiBufferFree(pServer);

	return bResult;
}


void CShowActiveDirUsers::GetDCName(LPTSTR szDCName, LPTSTR szDomain)
{
	NET_API_STATUS	ret = 0;
	LPBYTE			bufptr;
	SERVER_INFO_100* pS100;
	LPWSTR			pServer = NULL;
	DWORD			dwEntriesRead = 0;
	DWORD			dwTotalEntries = 0;

	ret =  NetGetDCName(NULL, NULL, (LPBYTE*)&pServer);
	if (ret == NERR_Success)
	{
		_tcscpy(szDCName, pServer);
		NetApiBufferFree(pServer);
		ret = NetGetDCName(szDCName, szDomain, (LPBYTE*)&pServer);
		if (ret == NERR_Success)
		{
			_tcscpy(szDCName, pServer);
			NetApiBufferFree(pServer);
			return;
		}
		return;
	}
	
	ret = NetServerEnum(NULL, 100, &bufptr, MAX_PREFERRED_LENGTH, 
							&dwEntriesRead, &dwTotalEntries, 
							SV_TYPE_DOMAIN_BAKCTRL, szDomain, 0);
	if (ret == NERR_Success && dwEntriesRead > 0)
	{
		pS100 = (SERVER_INFO_100*)bufptr;
		_tcscpy(szDCName, _T("\\\\"));
		_tcscat(szDCName, pS100->sv100_name);
		NetApiBufferFree(bufptr);
		return;
	}
}


void CShowActiveDirUsers::GetLocalComputerName(LPTSTR szComputerName)
{
	DWORD size = MAX_COMPUTERNAME_LENGTH + 1;

	ZeroMemory(szComputerName, size);
	GetComputerName(szComputerName, &size);
}


void CShowActiveDirUsers::GetUserAndDomainNameFromUPN(LPTSTR szUser, 
								LPTSTR szUserName, LPTSTR szDomainName)
{
	ULONG size = 8192;
	TCHAR buffer[8192];

	if ( TranslateName( szUser, NameUserPrincipal, NameSamCompatible, 
										buffer, &size ) )
	{
		// we UPN name
		TCHAR  szSeparators[] = L"\\";
		TCHAR* szToken  = L"";

		szToken = _tcstok( buffer, szSeparators );

		// domain
		_tcsupr(szToken);
		_tcscpy(szDomainName, szToken);

		// user name
		szToken = wcstok( NULL, szSeparators );
		_tcslwr(szToken);
		_tcscpy(szUserName, szToken);

	}
}


