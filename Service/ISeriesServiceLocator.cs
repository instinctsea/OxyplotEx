namespace OxyplotEx.Service
{
    interface ISeriesServiceLocator
    {
        T GetInstance<T>();
    }
}
