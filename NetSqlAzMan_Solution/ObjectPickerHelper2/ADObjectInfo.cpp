#include "stdafx.h"
#include "ADObjectInfo.h"

STDMETHODIMP CADObjectInfo::get_Name(BSTR* pVal)
{
	*pVal = m_bstrName.Copy();
	return S_OK;
}

STDMETHODIMP CADObjectInfo::get_Class(BSTR* pVal)
{
	*pVal = m_bstrClass.Copy();
	return S_OK;
}

STDMETHODIMP CADObjectInfo::get_ADPath(BSTR* pVal)
{
	*pVal = m_bstrADPath.Copy();
	return S_OK;
}

STDMETHODIMP CADObjectInfo::get_UPN(BSTR* pVal)
{
	*pVal = m_bstrUPN.Copy();
    return S_OK;
}

STDMETHODIMP CADObjectInfo::get_ObjectSID(VARIANT* pVal)
{
	*pVal = m_bstrObjectSID;
    return S_OK;
}


STDMETHODIMP CADObjectInfo::put_Name(BSTR pVal)
{
	m_bstrName = pVal;
	return S_OK;
}

STDMETHODIMP CADObjectInfo::put_Class(BSTR pVal)
{
	m_bstrClass = pVal;
	return S_OK;
}

STDMETHODIMP CADObjectInfo::put_ADPath(BSTR pVal)
{
	m_bstrADPath = pVal;
	return S_OK;
}

STDMETHODIMP CADObjectInfo::put_UPN(BSTR pVal)
{
	m_bstrUPN = pVal;
    return S_OK;
}

STDMETHODIMP CADObjectInfo::put_ObjectSID(VARIANT pVal)
{
	m_bstrObjectSID = pVal;
    return S_OK;
}