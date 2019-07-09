
using System;

namespace Battle.Core {

public enum Color {
    green,
    red,
    blue,
    yellow,
    white,
    cyan
}

public delegate void DrawLineDelegate(Vector start, Vector end, Color color, Number duration);
public delegate void LogDelegate(string message, params object[] args);
public delegate void AssertDelegate(bool expr, object message);
public delegate void BeginSampleDelegate(string info);
public delegate void EndSampleDelegate();

public class Debug {
    public static LogDelegate Log;
    public static LogDelegate LogWarn;
    public static LogDelegate LogError;

    public static void LogException(Exception e) {
        LogError(e.Message + "\n" + e.StackTrace);
    }

    public static AssertDelegate Assert;

    public static DrawLineDelegate DrawLine;

    public static BeginSampleDelegate BeginSample;
    public static EndSampleDelegate EndSample;

    public static void DrawBox(Vector point, Vector size, Color color, Number duration) {
        DrawLine(new Vector(point.x-size.x, point.y+size.y), new Vector(point.x+size.x, point.y+size.y), color, duration);
        DrawLine(new Vector(point.x+size.x, point.y+size.y), new Vector(point.x+size.x, point.y-size.y), color, duration);
        DrawLine(new Vector(point.x+size.x, point.y-size.y), new Vector(point.x-size.x, point.y-size.y), color, duration);
        DrawLine(new Vector(point.x-size.x, point.y-size.y), new Vector(point.x-size.x, point.y+size.y), color, duration);
    }
}

}