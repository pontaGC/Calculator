namespace SimpleCalculator.Core.Injectors
{
    /// <summary>
    /// Life style of a registered instance in the <see cref="IIoCContainer"/>.
    /// </summary>
    public enum InstanceLifeStyle
    {
        Transient,
        Singleton,
    }
}
