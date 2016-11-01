using System;
using System.Web;
using System.Web.Mvc;
using HyperQueryEF.Core;

public class LikeMyDessertHttpModule : IHttpModule
{
    public void Init(HttpApplication application)
    {
        application.BeginRequest += (new EventHandler(this.BeginRequest));
        application.EndRequest += (new EventHandler(this.EndRequest));
    }

    private void BeginRequest(Object source, EventArgs e)
    {

    }

    private void EndRequest(Object source, EventArgs e)
    {
        var _unitOfWork = DependencyResolver.Current.GetService<IUnitOfWork>();

        if (!_unitOfWork.HasChanges())
            return;

        try
        {
            _unitOfWork.SaveChanges();
        }
        catch (Exception ex)
        {
                
        }
    }

    public void Dispose()
    {

    }
}