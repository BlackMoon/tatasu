﻿using System.Windows;
using System.Xml;

namespace PluginInterface
{
    /// <summary>
    /// Базовый интерфейс плагинов
    /// </summary>
    public interface IPlugin
    {
        string Node { get; }                   // тип узла модели (Parameter, Commands, Equipments)
        int Version { get; }                   // версия плагина        

        string GetNodeName(XmlNode nd);
        UIElement GetEditor(XmlNode nd);
        XmlNode Save(UIElement control, XmlNode nd);
    }
}
