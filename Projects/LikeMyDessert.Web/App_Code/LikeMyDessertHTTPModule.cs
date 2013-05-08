using System;
using System.Web;

using LikeMyDessert.Repositories;

public class LikeMyDessertHTTPModule : IHttpModule
{
    public void Init(HttpApplication application)
    {
        application.BeginRequest += (new EventHandler(this.BeginRequest));
        application.EndRequest += (new EventHandler(this.EndRequest));
    }

    private void BeginRequest(Object source, EventArgs e)
    {
        PersistenceManager.OpenSession();
    }

    private void EndRequest(Object source, EventArgs e)
    {
        PersistenceManager.CommitCachedObjects();
        PersistenceManager.DisposeCache();
    }

    public void Dispose()
    {
    }
}
