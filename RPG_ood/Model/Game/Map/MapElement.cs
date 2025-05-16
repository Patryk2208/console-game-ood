using System.Text.Json;
using System.Text.Json.Serialization;
using RPG_ood.Model;
using RPG_ood.View;

namespace RPG_ood.Map;

[JsonDerivedType(typeof(BlankMapElement), "meB")]
[JsonDerivedType(typeof(Wall), "meW")]
public abstract class MapElement
{
    public abstract bool OnStandable { get; set; }
    
    public abstract void AcceptView(IViewGenerator generator);
}

public class BlankMapElement : MapElement
{
    public override bool OnStandable { get; set; } = true;
    public override void AcceptView(IViewGenerator generator)
    {
        generator.VisitBlankMapElement(this);
    }
}

public class Wall : MapElement
{
    public override bool OnStandable { get; set; } = false;
    public override void AcceptView(IViewGenerator generator)
    {
        generator.VisitWall(this);
    }
}