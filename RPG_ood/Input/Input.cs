using RPG_ood.Display;
using RPG_ood.Map;
using RPG_ood.Beings;
using RPG_ood.Game;

namespace RPG_ood.Input;

public class Input
{
    public ConsoleInputHandlerLink ChainOfResponsibilityHandler { private get; set; }
    private CancellationTokenSource _cts;
    private Mutex _mutex;
    public Input(Mutex mutex, CancellationTokenSource cancellationTokenSource)
    {
        _cts = cancellationTokenSource;
        _mutex = mutex;
        ChainOfResponsibilityHandler = null!;
    }
    
    public void TakeInput()
    {
        Task.Run(
            () =>
            {
                while (_cts.Token.IsCancellationRequested == false)
                {
                    var key = Console.ReadKey(true);
                    _mutex.WaitOne();
                    ChainOfResponsibilityHandler.HandleInput(key);
                    _mutex.ReleaseMutex();
                }
            }, _cts.Token);
    }

    private void ParseInput(ConsoleKeyInfo key)
    {
        /*string? newMessage = null;
        bool canMove;
        switch (key.Key)
        {
            case ConsoleKey.W:
            {
                try
                {
                    canMove = Game.CurrentRoom.Elements[P.Pos.X - 1, P.Pos.Y].OnStandable;
                }
                catch (Exception e) when (e is IndexOutOfRangeException)
                {
                    canMove = false;
                }

                if (canMove)
                {
                    Game.CurrentRoom.Elements[P.Pos.X, P.Pos.Y].OnStandable = true;
                    P.Pos = P.Pos with { X = P.Pos.X - 1, Y = P.Pos.Y };
                    Game.CurrentRoom.Elements[P.Pos.X, P.Pos.Y].OnStandable = false;
                    //newMessage = $"{P.Name} Moved Up";
                }

                break;
            }
            case ConsoleKey.S:
            {
                try
                {
                    canMove = Game.CurrentRoom.Elements[P.Pos.X + 1, P.Pos.Y].OnStandable;
                }
                catch (Exception e) when (e is IndexOutOfRangeException)
                {
                    canMove = false;
                }

                if (canMove)
                {
                    Game.CurrentRoom.Elements[P.Pos.X, P.Pos.Y].OnStandable = true;
                    P.Pos = P.Pos with { X = P.Pos.X + 1, Y = P.Pos.Y };
                    Game.CurrentRoom.Elements[P.Pos.X, P.Pos.Y].OnStandable = false;
                    //newMessage = $"{P.Name} Moved Down";
                }

                break;
            }
            case ConsoleKey.A:
            {
                try
                {
                    canMove = Game.CurrentRoom.Elements[P.Pos.X, P.Pos.Y - 1].OnStandable;
                }
                catch (Exception e) when (e is IndexOutOfRangeException)
                {
                    canMove = false;
                }

                if (canMove)
                {
                    Game.CurrentRoom.Elements[P.Pos.X, P.Pos.Y].OnStandable = true;
                    P.Pos = P.Pos with { X = P.Pos.X, Y = P.Pos.Y - 1 };
                    Game.CurrentRoom.Elements[P.Pos.X, P.Pos.Y].OnStandable = false;
                    //newMessage = $"{P.Name} Moved Left";
                }

                break;
            }
            case ConsoleKey.D:
            {
                try
                {
                    canMove = Game.CurrentRoom.Elements[P.Pos.X, P.Pos.Y + 1].OnStandable;
                }
                catch (Exception e) when (e is IndexOutOfRangeException)
                {
                    canMove = false;
                }

                if (canMove)
                {
                    Game.CurrentRoom.Elements[P.Pos.X, P.Pos.Y].OnStandable = true;
                    P.Pos = P.Pos with { X = P.Pos.X, Y = P.Pos.Y + 1 };
                    Game.CurrentRoom.Elements[P.Pos.X, P.Pos.Y].OnStandable = false;
                    //newMessage = $"{P.Name} Moved Right";
                }

                break;
            }
            case ConsoleKey.E:
            {
                if (Game.CurrentRoom.Items.Any())
                {
                    var itemsAtPos = Game.CurrentRoom.GetItemsAtPos(P.Pos).ToList();
                    if (itemsAtPos.Any())
                    {
                        var item = itemsAtPos.ElementAt(Game.CurrentRoom.PickUpCursor);
                        newMessage = $"{P.Name} picked up {item.Name} at {item.Pos}";
                        item.Pos = item.Pos with { X = -1, Y = -1 };
                        P.PickUpItem(item);
                    }
                }

                break;
            }
            case ConsoleKey.T:
            {
                if (P.Eq.Eq.Count > 0)
                {
                    newMessage = $"{P.Name} dropped {P.Eq.Eq[P.Eq.EqPointer].Name}";
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
                Game.CurrentRoom.PickUpCursor++;
                break;
            }
            case ConsoleKey.UpArrow:
            {
                Game.CurrentRoom.PickUpCursor = Math.Max(0, Game.CurrentRoom.PickUpCursor - 1);
                break;
            }
            case ConsoleKey.P:
            {
                if (!P.Bd.BodyParts.ContainsKey("RightHand")) break;
                if (P.Bd.BodyParts["RightHand"].IsUsed)
                {
                    P.TryTakeOffItem(P.Bd.BodyParts["RightHand"]);
                    newMessage = $"{P.Name} put {P.Eq.Eq[P.Eq.EqPointer].Name} from right hand back to eq";
                }
                else
                {
                    if (P.Eq.Eq.Count == 0) break;
                    newMessage = $"{P.Name} took {P.Eq.Eq[P.Eq.EqPointer].Name} to right hand";
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
                    newMessage = $"{P.Name} put {P.Eq.Eq[P.Eq.EqPointer].Name} from left hand back to eq";
                }
                else
                {
                    if (P.Eq.Eq.Count == 0) break;
                    newMessage = $"{P.Name} took {P.Eq.Eq[P.Eq.EqPointer].Name} to left hand";
                    P.TryTakeItem(P.Bd, "LeftHand");
                }
                break;
            }
            case ConsoleKey.Escape:
            {
                _cts.Cancel();
                break;
            }
        }
        if (newMessage != null)
        {
            Logs.AddLogMessage(newMessage!);
            var logDisp = Display.Display.GetInstance();
            logDisp.DisplayLog(Logs);
        }*/
    }
}