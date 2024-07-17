using System.Reflection;

namespace MicroService.Template.MSSql;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
