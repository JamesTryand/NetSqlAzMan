// ADObjectColl.h : Declaration of the CADObjectColl

#pragma once
#include "resource.h"       // main symbols
#include "ADObjectInfo.h"

#include "ObjectPickerHelper2.h"


#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif



// CADObjectColl

class ATL_NO_VTABLE CADObjectColl :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CADObjectColl, &CLSID_ADObjectColl>,
	public IDispatchImpl<IADObjectColl, &IID_IADObjectColl, &LIBID_ObjectPickerHelper2Lib, /*wMajor =*/ 1, /*wMinor =*/ 0>
{
public:
	CADObjectColl()
	{
	}

DECLARE_REGISTRY_RESOURCEID(IDR_ADOBJECTCOLL)

DECLARE_NOT_AGGREGATABLE(CADObjectColl)

BEGIN_COM_MAP(CADObjectColl)
	COM_INTERFACE_ENTRY(IADObjectColl)
	COM_INTERFACE_ENTRY(IDispatch)
END_COM_MAP()



	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}

	void FinalRelease() 
	{
		Zap();
	}
	void Zap()
	{
		// cleanup of the dispatch pointers we are storing
		IADObjectInfo* p;

		std::vector<IADObjectInfo*>::iterator i;
		std::vector<IADObjectInfo*>::iterator begin = m_coll.begin();
		std::vector<IADObjectInfo*>::iterator end = m_coll.end();
		for (i = begin; i !=end; ++i)
		{
			p = *i;
			p->Release();
		}
		
		m_coll.resize(0);
	}

public:

	STDMETHOD(get_Count)(ULONG* pVal);
	STDMETHOD(get__NewEnum)(/*[out, retval]*/ LPUNKNOWN *pVal);
	STDMETHOD(Item)(/*[in]*/ long index, /*[out, retval]*/ IADObjectInfo**  pVal);
	STDMETHOD(Add)(IADObjectInfo** ppObjInfo);

protected:
	typedef std::vector<IADObjectInfo*> ADObjectInfoVector;
	ADObjectInfoVector	m_coll;

public:
	STDMETHOD(RemoveAll)(void);

public:

};

OBJECT_ENTRY_AUTO(__uuidof(ADObjectColl), CADObjectColl)
