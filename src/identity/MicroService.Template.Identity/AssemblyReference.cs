using System.Reflection;

namespace MicroService.Template.Identity;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
