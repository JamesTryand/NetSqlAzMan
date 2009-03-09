#include "stdafx.h"
#include "ADObjectPicker.h"
#include "ShowActiveDirUsers.h"


STDMETHODIMP CADObjectPicker::get_ScopeTypeFlags(ULONG* pVal)
{
	*pVal = m_ScopeTypeFlags;
	return S_OK;
}

STDMETHODIMP CADObjectPicker::put_ScopeTypeFlags(ULONG newVal)
{
	m_ScopeTypeFlags = newVal;
	return S_OK;
}

STDMETHODIMP CADObjectPicker::get_ScopeFlags(ULONG* pVal)
{
	*pVal = m_ScopeFlags;
	return S_OK;
}

STDMETHODIMP CADObjectPicker::put_ScopeFlags(ULONG newVal)
{
	m_ScopeFlags = newVal;
	return S_OK;
}

STDMETHODIMP CADObjectPicker::get_UplevelFilterFlags_Both(ULONG* pVal)
{
	*pVal = m_UplevelFilterFlags_Both;	
	return S_OK;
}

STDMETHODIMP CADObjectPicker::put_UplevelFilterFlags_Both(ULONG newVal)
{
	m_UplevelFilterFlags_Both = newVal;	
	return S_OK;
}

STDMETHODIMP CADObjectPicker::get_UplevelFilterFlags_Mixed(ULONG* pVal)
{
	*pVal = m_UplevelFilterFlags_Mixed;
	return S_OK;
}

STDMETHODIMP CADObjectPicker::put_UplevelFilterFlags_Mixed(ULONG newVal)
{
	m_UplevelFilterFlags_Mixed = newVal;
	return S_OK;
}

STDMETHODIMP CADObjectPicker::get_UplevelFilterFlags_Native(ULONG* pVal)
{
	*pVal = m_UplevelFilterFlags_Native;
	return S_OK;
}

STDMETHODIMP CADObjectPicker::put_UplevelFilterFlags_Native(ULONG newVal)
{
	m_UplevelFilterFlags_Native = newVal;
	return S_OK;
}


STDMETHODIMP CADObjectPicker::get_DownLevelFilterFlags(ULONG* pVal)
{
	*pVal = m_DownlevelFilterFlags;
	return S_OK;
}

STDMETHODIMP CADObjectPicker::put_DownLevelFilterFlags(ULONG newVal)
{
	m_DownlevelFilterFlags = newVal;
	return S_OK;
}

STDMETHODIMP CADObjectPicker::get_ComputerName(BSTR* pVal)
{
	*pVal = m_bstrComputerName.Copy();
	return S_OK;
}

STDMETHODIMP CADObjectPicker::put_ComputerName(BSTR newVal)
{
	m_bstrComputerName = newVal;
	return S_OK;
}

STDMETHODIMP CADObjectPicker::get_InitInfo_OptionFlags(ULONG* pVal)
{
	*pVal = m_InitInfoFlags;	
	return S_OK;
}

STDMETHODIMP CADObjectPicker::put_InitInfo_OptionFlags(ULONG newVal)
{
	m_InitInfoFlags = newVal;
	return S_OK;
}

STDMETHODIMP CADObjectPicker::InvokeDialog(LONG hWnd)
{
	#pragma warning (disable: 4312)  // we know what we're doing
	UINT					i;
	DS_SELECTION			dsSelection;
	
	m_ADObjectColl->RemoveAll();

	// OK, show the dialog
	m_adUsers.Init((HWND)hWnd, this);
	if (!m_adUsers.ShowUserSelectionDialog())
		return S_FALSE;
	
	// decode
	DS_SELECTION_LIST* pList = m_adUsers.RetrieveUserSelectionList();
	if (NULL == pList)
		return S_FALSE;

	
	// walk the list and populate collection
	for (i=0; i<pList->cItems; i++)
	{
		dsSelection = pList->aDsSelection[i];
		{
			CComPtr<IADObjectInfo>  ptrObjInfo;
			m_ADObjectColl->Add(&ptrObjInfo);
			CComBSTR bstr;
			bstr = dsSelection.pwzName;
			ptrObjInfo->put_Name(bstr.Copy());
			bstr = dsSelection.pwzClass;
			ptrObjInfo->put_Class(bstr.Copy());
			bstr = dsSelection.pwzADsPath;
			ptrObjInfo->put_ADPath(bstr.Copy());
			bstr = dsSelection.pwzUPN;
			ptrObjInfo->put_UPN(bstr.Copy());
			//bstr = dsSelection.pvarFetchedAttributes[0].bstrVal;
			//bstr = V_BSTR(&dsSelection.pvarFetchedAttributes[0]);
			/*SAFEARRAY * barray = dsSelection.pvarFetchedAttributes[0].parray;
			bstr = (bstr)barray[0].cDims;*/
			ptrObjInfo->put_ObjectSID(dsSelection.pvarFetchedAttributes[0]);

		}
	}
	
	return S_OK;
}

STDMETHODIMP CADObjectPicker::get_ADObjectsColl(IDispatch** pVal)
{
	(m_ADObjectColl.p)->AddRef();
	*pVal = m_ADObjectColl;
	return S_OK;
}
