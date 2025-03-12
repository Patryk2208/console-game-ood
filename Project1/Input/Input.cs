using Project_oob.Beings;
using Project_oob.Display;
using Project_oob.Game;
using Project_oob.Map;

namespace Project_oob.Input;

public abstract class Input
{
    public abstract GameState Game { get; set; }
    public abstract Player P { get; set; }
    public abstract void TakeInput();
}

public class KeyboardInput : Input
{
    public override GameState Game { get; set; }
    public override Player P { get; set; }
    protected CancellationTokenSource _cts;
    protected Mutex _mutex;
    public KeyboardInput(GameState game, CancellationTokenSource cancellationTokenSource, Mutex mutex)
    {
        Game = game;
        P = (Player)game.Beings[0];
        _cts = cancellationTokenSource;
        _mutex = mutex;
    }
    
    public override void TakeInput()
    {
        Task.Run(
            () =>
            {
                while (_cts.Token.IsCancellationRequested == false)
                {
                    var key = Console.ReadKey(true);
                    _mutex.WaitOne();
                    ParseInput(key);
                    _mutex.ReleaseMutex();
                }
            }, _cts.Token);
    }

    private void ParseInput(ConsoleKeyInfo key)
    {
        bool canMove;
        switch (key.Key)
        {
            case ConsoleKey.W:
            {
                try
                {
                    canMove = Game.CurrentRoom.Elements[P.Pos.X - 1, P.Pos.Y].OnStandable();
                }
                catch (Exception e) when (e is IndexOutOfRangeException)
                {
                    canMove = false;
                }
                if (canMove)
                {
                    P.Pos = P.Pos with { X = P.Pos.X - 1, Y = P.Pos.Y };
                }
                break;
            }
            case ConsoleKey.S:
            {
                try
                {
                    canMove = Game.CurrentRoom.Elements[P.Pos.X + 1, P.Pos.Y].OnStandable();
                }
                catch (Exception e) when (e is IndexOutOfRangeException)
                {
                    canMove = false;
                }
                if (canMove)
                {
                    P.Pos = P.Pos with { X = P.Pos.X + 1, Y = P.Pos.Y };
                }
                break;
            }
            case ConsoleKey.A:
            {
                try
                {
                    canMove = Game.CurrentRoom.Elements[P.Pos.X, P.Pos.Y - 1].OnStandable();
                }
                catch (Exception e) when (e is IndexOutOfRangeException)
                {
                    canMove = false;
                }
                if (canMove)
                {
                    P.Pos = P.Pos with { X = P.Pos.X, Y = P.Pos.Y - 1 };
                }
                break;
            }
            case ConsoleKey.D:
            {
                try
                {
                    canMove = Game.CurrentRoom.Elements[P.Pos.X, P.Pos.Y + 1].OnStandable();
                }
                catch (Exception e) when (e is IndexOutOfRangeException)
                {
                    canMove = false;
                }
                if (canMove)
                {
                    P.Pos = P.Pos with { X = P.Pos.X, Y = P.Pos.Y + 1 };
                }
                break;
            }
            case ConsoleKey.E:
            {
                try
                {
                    var item = Game.GetItemsAtPos(P.Pos).ElementAt(Game.SamePosIndex);
                    item.Pos = item.Pos with { X = -1, Y = -1 };
                    P.PickUpItem(item);
                }
                catch (Exception)
                {
                    
                }
                break;
            }
            case ConsoleKey.T:
            {
                if (P.Eq.Eq.Count > 0)
                {
                    P.DropItem();
                }
                break;
            }
            case ConsoleKey.LeftArrow:
            {
                P.Eq.TryMovePointerLeft();
                break;
            }
            case ConsoleKey.RightArrow:
            {
                P.Eq.TryMovePointerRight();
                break;
            }
            case ConsoleKey.DownArrow:
            {
                Game.SamePosIndex++;
                break;
            }
            case ConsoleKey.UpArrow:
            {
                Game.SamePosIndex = Math.Max(0, Game.SamePosIndex - 1);
                break;
            }
            case ConsoleKey.P:
            {
                if(!P.Bd.BodyParts.ContainsKey("RightHand")) break;
                if (P.Bd.BodyParts["RightHand"].IsUsed)
                {
                    P.TryTakeOffItem(P.Bd.BodyParts["RightHand"]);
                }
                else
                {
                    if(P.Eq.Eq.Count == 0) break;
                    P.TryTakeItem(P.Bd, "RightHand");
                }
                break;
            }
            case ConsoleKey.L:
            {
                if (!P.Bd.BodyParts.ContainsKey("LeftHand")) break;
                if (P.Bd.BodyParts["LeftHand"].IsUsed)
                {
                    P.TryTakeOffItem(P.Bd.BodyParts["LeftHand"]);
                }
                else
                {
                    if(P.Eq.Eq.Count == 0) break;
                    P.TryTakeItem(P.Bd, "LeftHand");
                }
                break;
            }
            /*case ConsoleKey.O:
            {
                if (!P.Bd.BodyParts.ContainsKey("RightHand") || !P.Bd.BodyParts.ContainsKey("LeftHand")) break;
                if (P.Bd.BodyParts["RightHand"].IsUsed || P.Bd.BodyParts["LeftHand"].IsUsed)
                {
                    P.TryTakeOffItem([P.Bd.BodyParts["RightHand"], P.Bd.BodyParts["LeftHand"]]);
                }
                else
                {
                    if(P.Eq.Eq.Count == 0) break;
                    P.TryTakeItem([P.Bd.BodyParts["RightHand"], P.Bd.BodyParts["LeftHand"]]);
                }
                break;
            }*/
            case ConsoleKey.Escape:
            {
                _cts.Cancel();
                break;
            }
        }
    }
}