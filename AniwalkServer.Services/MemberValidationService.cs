using AniwalkServer.Data;

namespace AniwalkServer.Services;

/// <summary>
/// 
/// </summary>
public class MemberValidationService
{
    private readonly AniwalkDBContext Context;

    public MemberValidationService(AniwalkDBContext Context)
    {
        this.Context = Context;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Name"></param>
    /// <returns></returns>
    public bool IsNameTaken(string Name)
    {
        return Context.Members.Any(m => m.Name == Name);
    }
}