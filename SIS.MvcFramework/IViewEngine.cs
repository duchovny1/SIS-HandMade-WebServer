namespace SIS.MvcFramework
{
    public  interface IViewEngine
    {
        string GetHtml(string template, object model);

    }
}
