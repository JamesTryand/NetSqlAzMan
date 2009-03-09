// ADObjectInfo.h : Declaration of the CADObjectInfo

#pragma once
#include "resource.h"       // main symbols

#include "ObjectPickerHelper2.h"


#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif



// CADObjectInfo

class ATL_NO_VTABLE CADObjectInfo :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CADObjectInfo, &CLSID_ADObjectInfo>,
	public IDispatchImpl<IADObjectInfo, &IID_IADObjectInfo, &LIBID_ObjectPickerHelper2Lib, /*wMajor =*/ 1, /*wMinor =*/ 0>
{
public:
	CADObjectInfo()
	{
		m_bstrName = _T("");
		m_bstrClass = _T("");
		m_bstrADPath = _T("");
		m_bstrUPN = _T("");
		//m_bstrObjectSID = _T("");
	}

DECLARE_REGISTRY_RESOURCEID(IDR_ADOBJECTINFO)

DECLARE_NOT_AGGREGATABLE(CADObjectInfo)

BEGIN_COM_MAP(CADObjectInfo)
	COM_INTERFACE_ENTRY(IADObjectInfo)
	COM_INTERFACE_ENTRY(IDispatch)
END_COM_MAP()



	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}

	void FinalRelease()
	{
	}

public:
	STDMETHOD(get_Name)(BSTR* pVal);
	STDMETHOD(put_Name)(BSTR pVal);
	STDMETHOD(get_Class)(BSTR* pVal);
	STDMETHOD(put_Class)(BSTR pVal);
	STDMETHOD(get_ADPath)(BSTR* pVal);
	STDMETHOD(put_ADPath)(BSTR pVal);
	STDMETHOD(get_UPN)(BSTR* pVal);
	STDMETHOD(put_UPN)(BSTR pVal);
	STDMETHOD(get_ObjectSID)(VARIANT* pVal);
	STDMETHOD(put_ObjectSID)(VARIANT pVal);


protected:
	CComBSTR m_bstrName;
	CComBSTR m_bstrClass;
	CComBSTR m_bstrADPath;
	CComBSTR m_bstrUPN;
	VARIANT m_bstrObjectSID;

};

OBJECT_ENTRY_AUTO(__uuidof(ADObjectInfo), CADObjectInfo)
