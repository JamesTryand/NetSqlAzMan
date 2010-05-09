
namespace NetSqlAzMan
{
    /// <summary>
    /// BeforeCheckAccessHandler delegate.
    /// </summary>
    /// <param name="context">The NetSqlAzMan Security Context</param>
    /// <param name="attribute">The attribute</param>
    public delegate void BeforeCheckAccessHandler(NetSqlAzManAuthorizationContext context, NetSqlAzManAuthorizationAttribute attribute);
    /// <summary>
    /// AfterCheckAccessHandler delegate.
    /// </summary>
    /// <param name="context">The NetSqlAzMan Security Context</param>
    /// <param name="attribute">The attribute</param>
    /// <param name="partialResult">the partial check access result</param>
    public delegate void AfterCheckAccessHandler(NetSqlAzManAuthorizationContext context, NetSqlAzManAuthorizationAttribute attribute, ref bool partialResult);
}
