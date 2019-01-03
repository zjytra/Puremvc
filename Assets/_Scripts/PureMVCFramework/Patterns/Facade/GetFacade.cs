using PureMVC.Patterns;

public  class GetFacade
{

    protected Facade Facade
    {
        get { return m_facade; }
    }
    /// <summary>
    /// Local reference to the Facade Singleton
    /// </summary>
    private Facade m_facade = PureMVC.Patterns.Facade.Instance;

}

