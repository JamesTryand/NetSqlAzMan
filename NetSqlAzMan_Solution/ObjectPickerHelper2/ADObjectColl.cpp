#include "stdafx.h"
#include "ADObjectColl.h"
#include "ADObjectInfo.h"
#include "ADObjectPicker.h"


STDMETHODIMP CADObjectColl::get_Count(ULONG* pVal)
{
	if (m_coll.empty())
		*pVal = 0;
	else
		*pVal = (ULONG)m_coll.size();
	return S_OK;
}

struct _CopyVariantFromADObjectInfo
{
	static HRESULT copy(VARIANT* p1, IADObjectInfo* const* p2)
	{
		p1->vt = VT_DISPATCH;
		p1->pdispVal = *p2;
		p1->pdispVal->AddRef();
		return S_OK;
	}
	static void init(VARIANT* p){VariantInit(p);}
	static void destroy(VARIANT* p){VariantClear(p);}
};

STDMETHODIMP CADObjectColl::get__NewEnum(LPUNKNOWN *pVal)
{
	
	typedef CComEnumOnSTL<IEnumVARIANT, 
					  &IID_IEnumVARIANT, 
					  VARIANT, 
					  _CopyVariantFromADObjectInfo, 
					  ADObjectInfoVector > 
			CComEnumVariantOnVector;

	CComObject<CComEnumVariantOnVector>* pe = 0;		// our enumerator

	// create it
	HRESULT hr = CComObject<CComEnumVariantOnVector>::CreateInstance(&pe);
	if (SUCCEEDED(hr))
		pe->AddRef();

	// copy the data from our vector to it
	hr = pe->Init(this->GetUnknown(), m_coll);

	// and hand to caller
	if (SUCCEEDED(hr))
		hr = pe->QueryInterface(pVal);

	// cleanup
	pe->Release();

	return S_OK;
}

STDMETHODIMP CADObjectColl::Item(long index, IADObjectInfo** pVal)
{
		// sanity check
	if (index < 1 || index > (long)m_coll.size())
		return E_INVALIDARG;

	// vb is 1 based
	*pVal = m_coll[index - 1];

	// handing out a copy, so addref it
	(*pVal)->AddRef();  
	
	return S_OK;
}


STDMETHODIMP CADObjectColl::Add(IADObjectInfo** ppObjInfo)
{
	CComPtr<IADObjectInfo> ptr;			// the object
	IADObjectInfo * pInfo = NULL;		// the interface pointer
	HRESULT		hr;
	
	// init
	*ppObjInfo = NULL;

	// create the object
	hr = ptr.CoCreateInstance (__uuidof(ADObjectInfo));
	if (!SUCCEEDED(hr))
		return E_FAIL;

	// addref because we're handing out the pointer
	(ptr.p)->QueryInterface(IID_IADObjectInfo, (void**)&pInfo);  

	// addref because we are storing it, released on destructor
	pInfo->AddRef(); 

	// add to stl vector
	m_coll.push_back(pInfo);

	// and return copy to caller
	*ppObjInfo = pInfo;
	return S_OK;
}


STDMETHODIMP CADObjectColl::RemoveAll(void)
{
	Zap();
	return S_OK;
}
