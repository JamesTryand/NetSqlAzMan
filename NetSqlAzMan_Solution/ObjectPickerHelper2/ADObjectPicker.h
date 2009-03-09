// ADObjectPicker.h : Declaration of the CADObjectPicker

#pragma once
#include "resource.h"       // main symbols
#include "ShowActiveDirUsers.h"
#include "ObjectPickerHelper2.h"


#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif



// CADObjectPicker

class ATL_NO_VTABLE CADObjectPicker :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CADObjectPicker, &CLSID_ADObjectPicker>,
	public IDispatchImpl<IADObjectPicker, &IID_IADObjectPicker, &LIBID_ObjectPickerHelper2Lib, /*wMajor =*/ 1, /*wMinor =*/ 0>
{
public:
	CADObjectPicker()
	{
	}

DECLARE_REGISTRY_RESOURCEID(IDR_ADOBJECTPICKER)

DECLARE_NOT_AGGREGATABLE(CADObjectPicker)

BEGIN_COM_MAP(CADObjectPicker)
	COM_INTERFACE_ENTRY(IADObjectPicker)
	COM_INTERFACE_ENTRY(IDispatch)
END_COM_MAP()



	DECLARE_PROTECT_FINAL_CONSTRUCT()

		HRESULT FinalConstruct()
	{
		HRESULT hr;
		hr = m_ADObjectColl.CoCreateInstance(__uuidof(ADObjectColl));
		// provide reasonable defaults USERS, GROUPS
		m_ScopeTypeFlags = 0x37f;
		m_ScopeFlags = 0xc3;
		m_UplevelFilterFlags_Both = 0x42;
		m_UplevelFilterFlags_Mixed = 0;
		m_UplevelFilterFlags_Native = 0;
		m_DownlevelFilterFlags = (ULONG)0x800000005;
		m_InitInfoFlags = 0x3;
		return hr;
	}


	void FinalRelease()
	{
	}

public:
	STDMETHOD(get_ScopeTypeFlags)(ULONG* pVal);
	STDMETHOD(put_ScopeTypeFlags)(ULONG newVal);
	STDMETHOD(get_ScopeFlags)(ULONG* pVal);
	STDMETHOD(put_ScopeFlags)(ULONG newVal);
	STDMETHOD(get_UplevelFilterFlags_Both)(ULONG* pVal);
	STDMETHOD(put_UplevelFilterFlags_Both)(ULONG newVal);
	STDMETHOD(get_UplevelFilterFlags_Mixed)(ULONG* pVal);
	STDMETHOD(put_UplevelFilterFlags_Mixed)(ULONG newVal);
	STDMETHOD(get_UplevelFilterFlags_Native)(ULONG* pVal);
	STDMETHOD(put_UplevelFilterFlags_Native)(ULONG newVal);
	STDMETHOD(get_DownLevelFilterFlags)(ULONG* pVal);
	STDMETHOD(put_DownLevelFilterFlags)(ULONG newVal);
	STDMETHOD(get_ComputerName)(BSTR* pVal);
	STDMETHOD(put_ComputerName)(BSTR newVal);
	STDMETHOD(get_InitInfo_OptionFlags)(ULONG* pVal);
	STDMETHOD(put_InitInfo_OptionFlags)(ULONG newVal);
	STDMETHOD(InvokeDialog)(LONG hWnd);
	STDMETHOD(get_ADObjectsColl)(IDispatch** pVal);

protected:
	friend class CShowActiveDirUsers;
	ULONG					m_ScopeTypeFlags;
	ULONG					m_ScopeFlags;
	ULONG					m_UplevelFilterFlags_Both;
	ULONG					m_UplevelFilterFlags_Mixed;
	ULONG					m_UplevelFilterFlags_Native;
	ULONG					m_DownlevelFilterFlags;
	CComBSTR				m_bstrComputerName;
	ULONG					m_InitInfoFlags;
	CComPtr<IADObjectColl>	m_ADObjectColl;
	CShowActiveDirUsers		m_adUsers;


};

OBJECT_ENTRY_AUTO(__uuidof(ADObjectPicker), CADObjectPicker)
