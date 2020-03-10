namespace Inheritance.MapObjects
{
    interface ITreasure
    {
        Treasure Treasure { get; set; }
    }

    interface IMapObject
    {
        int Owner { get; set; }
    }

    interface IBattle
    {
        Army Army { get; set; }
    }

    public class Dwelling : IMapObject
    {
        public int Owner { get; set; }
    }

    public class Mine : IBattle, ITreasure, IMapObject
    {
        public int Owner { get; set; }
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Creeps : IBattle, ITreasure
    {
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Wolfs : IBattle
    {
        public Army Army { get; set; }
    }

    public class ResourcePile : ITreasure
    {
        public Treasure Treasure { get; set; }
    }

    public static class Interaction
    {
        public static void Make(Player player, object mapObject)
        {
            if (mapObject is IBattle enemy && !player.CanBeat(enemy.Army))
            {
                player.Die();
                return;
            }
            if (mapObject is ITreasure treasure)
                player.Consume(treasure.Treasure);
            if (mapObject is IMapObject obj)
                obj.Owner = player.Id;
        }
    }
}