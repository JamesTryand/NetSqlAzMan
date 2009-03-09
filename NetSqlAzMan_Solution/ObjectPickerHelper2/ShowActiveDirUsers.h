
#if !defined(AFX_SHOWACTIVEDIRUSERS_H__340C47DC_7D22_4D8C_95AE_C40AEBBBE00F__INCLUDED_)
#define AFX_SHOWACTIVEDIRUSERS_H__340C47DC_7D22_4D8C_95AE_C40AEBBBE00F__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


#include "objsel.h"
#include "ObjectPickerHelper2.h"

class CADObjectPicker;

class CShowActiveDirUsers  
{
public:
	CShowActiveDirUsers();
	CShowActiveDirUsers(HWND hWnd, CADObjectPicker* pObjPicker);
	void Init(HWND hWnd, CADObjectPicker* pObjPicker);
	virtual ~CShowActiveDirUsers();
	
	// public helpers
	bool ShowUserSelectionDialog();
	BOOL GetUserInfoFromActiveDirectorySelection(DS_SELECTION& ds, 
				LPTSTR szUser, LPTSTR szDomain, LPTSTR szFullUserName, 
				LPTSTR szDescription);
	BOOL GetUserAndDomainFromADsPath(LPTSTR szADsPath, LPTSTR szUser, 
				LPTSTR szDomain);
	void GetDCName(LPTSTR szDCName, LPTSTR szDomain);
	DS_SELECTION_LIST* RetrieveUserSelectionList();

protected:
	
	bool IsOSVersionOK();
	BOOL GetFullUserName(LPTSTR szUser, LPTSTR szDomain, 
			LPTSTR szFullUserName, LPTSTR szDescription);
	void GetLocalComputerName(LPTSTR szComputerName);
	void GetUserAndDomainNameFromUPN(LPTSTR szUser, LPTSTR szUserName, 
			LPTSTR szDomainName);
	
	HWND			m_hWnd;				// window handle
	IDataObjectPtr  m_pDataObject;		// data object
	STGMEDIUM		m_stg;				// storage medium
	CADObjectPicker* m_pObjPicker;
};

#endif // !defined(AFX_SHOWACTIVEDIRUSERS_H__340C47DC_7D22_4D8C_95AE_C40AEBBBE00F__INCLUDED_)
