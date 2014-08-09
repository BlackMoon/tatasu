
namespace PluginInterface
{
    /// <summary>
    /// Базовый интерфейс плагинов
    /// </summary>
    public interface IPlugin
    {
        string Node { get; }                   // тип узла модели (Parameter, Commands, Equipments)
        int Version { get; }                   // версия плагина
    }
}
