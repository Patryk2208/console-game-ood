using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using RPG_ood.Beings;
using RPG_ood.Game;
using RPG_ood.Items;

namespace RPG_ood.Map;


public interface IRoomBuilder : IMapBuilder
{
    public void BuildEmptyRoom();
    public void BuildFullRoom(int color);
    public void CarveMaze();
    public (int, int) AddRandomPath(int s0 = -1, int s1 = -1);
    public void AddRandomChamber(int size);
    public void AddCentralRoom(float size);

    public void PlaceItems(int maxItemsOfType);
    public void PlaceWeapons(int maxItemsOfType);
    public void PlaceModifiedWeapons(int maxItemsOfType);
    public void PlaceElixirs(int maxItemsOfType);
    public void PlacePlayer(Player player);
    public void PlaceEnemies(int maxItemsOfType);
}

public class RoomBuilder(string roomName, int width, int height) : IRoomBuilder
{
    private Room _room = new Room(roomName, width, height);
    private Random _random = new(); 
    private void Place(ItemFactory factory, int maxItemsOfType)
    {
        var emptySpaces = new List<(int, int)>();
        for (int i = 0; i < _room.Height; i++)
        {
            for (int j = 0; j < _room.Width; j++)
            {
                if (_room.Elements[i, j].OnStandable)
                {
                    emptySpaces.Add((i, j));
                }
            }
        }
        for (int i = 0; i < maxItemsOfType; i++)
        {
            int ind = _random.Next(emptySpaces.Count);
            var it = factory.CreateItem();
            it.Pos = new Position(emptySpaces[ind].Item1, emptySpaces[ind].Item2);
            _room.Items.Add(it);
        }
    }
    
    private void Spawn(IBeingFactory factory, int maxItemsOfType)
    {
        var emptySpaces = new List<(int, int)>();
        for (int i = 0; i < _room.Height; i++)
        {
            for (int j = 0; j < _room.Width; j++)
            {
                if (_room.Elements[i, j].OnStandable)
                {
                    emptySpaces.Add((i, j));
                }
            }
        }
        for (int i = 0; i < maxItemsOfType; i++)
        {
            int ind = _random.Next(emptySpaces.Count);
            var b = factory.CreateBeing();
            b.Pos = new Position(emptySpaces[ind].Item1, emptySpaces[ind].Item2);
            _room.Elements[b.Pos.X, b.Pos.Y].OnStandable = false;
            _room.Beings.Add(b);
            emptySpaces.RemoveAt(ind);
        }
    }
    
    public void BuildEmptyRoom()
    {
        for (int i = 0; i < _room.Height; i++)
        {
            for (int j = 0; j < _room.Width; j++)
            {
                _room.Elements[i, j] = new BlankMapElement();
            }
        }
    }

    public void BuildFullRoom(int color)
    {
        for (int i = 0; i < _room.Height; i++)
        {
            for (int j = 0; j < _room.Width; j++)
            {
                _room.Elements[i, j] = new Wall(color);
            }
        }
    }
    
    public void CarveMaze() //Prim's algorithm implementation
    {
        var (p0, p1) = (0, 0);
        List<(int, int)> border = new();
        _room.Elements[p0, p1] = new BlankMapElement();
        TryAddNeighbors(border, p0, p1);
        while (border.Count > 0)
        {
            var branch = _random.Next(border.Count * 3);
            if(branch >= border.Count) branch = border.Count - 1;
            var (n0, n1) = (border[branch].Item1, border[branch].Item2);
            int top = -1, bottom = -1, left = -1, right = -1, neighbors = 0;
            bool isTop = n0 > 0, isBottom = n0 < _room.Height - 1, isLeft = n1 > 0, isRight = n1 < _room.Width - 1;
            if (isTop && _room.Elements[n0 - 1, n1].OnStandable)
            {
                top = 1;
                neighbors++;
            }

            if (isBottom && _room.Elements[n0 + 1, n1].OnStandable)
            {
                bottom = 1;
                neighbors++;
            }

            if (isLeft && _room.Elements[n0, n1 - 1].OnStandable)
            {
                left = 1;
                neighbors++;
            }

            if (isRight && _room.Elements[n0, n1 + 1].OnStandable)
            {
                right = 1;
                neighbors++;
            }

            if (neighbors == 1)
            {
                bool mayProceed = false;
                int topLeft = 1, topRight = 1, bottomLeft = 1, bottomRight = 1;
                if (isTop && isLeft && _room.Elements[n0 - 1, n1 - 1].OnStandable)
                {
                    topLeft = 0;
                }

                if (isTop && isRight && _room.Elements[n0 - 1, n1 + 1].OnStandable)
                {
                    topRight = 0;
                }

                if (isBottom && isLeft && _room.Elements[n0 + 1, n1 - 1].OnStandable)
                {
                    bottomLeft = 0;
                }

                if (isBottom && isRight && _room.Elements[n0 + 1, n1 + 1].OnStandable)
                {
                    bottomRight = 0;
                }

                if (top == 1 && bottomLeft == 1 && bottomRight == 1) mayProceed = true;
                if (left == 1 && topRight == 1 && bottomRight == 1) mayProceed = true;
                if (bottom == 1 && topLeft == 1 && topRight == 1) mayProceed = true;
                if (right == 1 && topLeft == 1 && bottomLeft == 1) mayProceed = true;
                if (mayProceed)
                {
                    _room.Elements[n0, n1] = new BlankMapElement();
                    TryAddNeighbors(border, n0, n1);
                }
            }

            border.RemoveAt(branch);
        }
    }

    private void TryAddNeighbors(List<(int, int)> l, int p0, int p1)
    {
        if (p0 > 0 && !l.Contains((p0-1, p1)) && !_room.Elements[p0 - 1, p1].OnStandable)
        {
            l.Add((p0 - 1, p1));
        }

        if (p0 < _room.Height - 1 && !l.Contains((p0 + 1, p1)) && !_room.Elements[p0 + 1, p1].OnStandable)
        {
            l.Add((p0 + 1, p1));
        }

        if (p1 > 0 && !l.Contains((p0, p1 - 1)) && !_room.Elements[p0, p1 - 1].OnStandable)
        {
            l.Add((p0, p1 - 1));
        }

        if (p1 < _room.Width - 1 && !l.Contains((p0, p1 + 1)) && !_room.Elements[p0, p1 + 1].OnStandable)
        {
            l.Add((p0, p1 + 1));
        }
    }

    public (int, int) AddRandomPath(int s0, int s1)
    {
        if(s0 < 0 || s1 < 0)
            (s0, s1) = (_random.Next(0, _room.Height), _random.Next(0, _room.Width));
        var (t0, t1) = (_random.Next(0, _room.Height), _random.Next(0, _room.Width));
        var currentPos = new Position(s0, s1);
        int xDirection = t0 >= s0 ? 1 : -1;
        int yDirection = t1 >= s1 ? 1 : -1;
        while (currentPos.X != t0 || currentPos.Y != t1)
        {
            if (currentPos.X == t0)
            {
                while (currentPos.Y != t1)
                {
                    _room.Elements[currentPos.X, currentPos.Y] = new BlankMapElement();
                    currentPos.Y += yDirection;
                }
                _room.Elements[currentPos.X, currentPos.Y] = new BlankMapElement();
                break;
            }
            else if (currentPos.Y == t1)
            {
                while (currentPos.X != t0)
                {
                    _room.Elements[currentPos.X, currentPos.Y] = new BlankMapElement();
                    currentPos.X += xDirection;
                }
                _room.Elements[currentPos.X, currentPos.Y] = new BlankMapElement();
                break;
            }
            _room.Elements[currentPos.X, currentPos.Y] = new BlankMapElement();
            if (_random.Next(0, 2) == 0)
            {
                currentPos.X += xDirection;
            }
            else
            {
                currentPos.Y += yDirection;
            }
        }
        return (t0, t1);
    }

    public void AddRandomChamber(int size)
    {
        var (s0, s1) = (_random.Next(0, _room.Height), _random.Next(0, _room.Width));
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if(s0 + i < _room.Height && s1 + j < _room.Width)
                    _room.Elements[s0 + i, s1 + j] = new BlankMapElement();
            }
        }
    }

    public void AddCentralRoom(float size)
    {
        int a = (int)(_room.Width *  size);
        int b = (int)(_room.Height * size);
        var (s0, s1) = ((_room.Height - b) / 2, (_room.Width - a) / 2);
        for (int i = s0; i < s0 + b; i++)
        {
            for (int j = s1; j < s1 + a; j++)
            {
                _room.Elements[i, j] = new BlankMapElement();
            }
        }
    }

    public void PlaceItems(int maxItemsOfType)
    {
        ItemFactory factory;
        for (int i = 0; i < maxItemsOfType; i++)
        {
            switch (_random.Next(2))
            {
                case 0:
                {
                    factory = new MoneyFactory(_random);
                    break;
                }
                case 1:
                {
                    factory = new MineralFactory(_random);
                    break;
                }
                default:
                {
                    throw new Exception("Random.Next() error.");
                }
            }
            Place(factory, 1);
        }
    }

    public void PlaceWeapons(int maxItemsOfType)
    {
        Place(new WeaponFactory(_random, false), maxItemsOfType);
    }

    public void PlaceModifiedWeapons(int maxItemsOfType)
    {
        Place(new WeaponFactory(_random, true), maxItemsOfType);
    }

    public void PlaceElixirs(int maxItemsOfType)
    {
        Place(new ElixirFactory(_random), maxItemsOfType);
    }

    public void PlacePlayer(Player player)
    {
        _room.Beings.Add(player);
        _room.Elements[player.Pos.X, player.Pos.Y].OnStandable = false;
    }

    public void PlaceEnemies(int maxItemsOfType)
    {
        IBeingFactory factory;
        factory = new EnemyFactory(_random);
        Spawn(factory, maxItemsOfType);
    }

    public Room GetResult()
    {
        return _room;
    }
}