using System;
using System.Collections.Generic;
using System.Text;

namespace Net.CrossCotting
{
    public static class Utilidades
    {
        public static DateTime DateTimeEmpty()
        {
            return new DateTime(1, 1, 1, 0, 0, 0);
        }

        public static DateTime GetFechaHoraInicioActual(DateTime? fecha)
        {
            DateTime? data;

            data = fecha;

            if (fecha == null || fecha.Equals(DateTimeEmpty()))
            {
                //data = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                data = new DateTime(DateTime.Now.Year, 1, DateTime.Now.Day, 0, 0, 0);
            } else
            {
                data = new DateTime(((DateTime)fecha).Year, ((DateTime)fecha).Month, ((DateTime)fecha).Day, 0, 0, 0);
            }

            return (DateTime)data;
        }

        public static DateTime GetFechaHoraFinActual(DateTime? fecha)
        {
            DateTime? data;

            data = fecha;

            if (fecha == null || fecha.Equals(DateTimeEmpty()))
            {
                data = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            } else
            {
                data = new DateTime(((DateTime)fecha).Year, ((DateTime)fecha).Month, ((DateTime)fecha).Day, 23, 59, 59);
            }

            return (DateTime)data; 
        }

        public static T Clone<T>(this T obj)
        {
            var inst = obj.GetType().GetMethod("MemberwiseClone", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            return (T)inst?.Invoke(obj, null);
        }
    }
}
