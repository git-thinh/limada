/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */


using System;
using System.Collections.Generic;
using Limaki.Actions;
using Limaki.View.Rendering;

namespace Limaki.View.UI {
    /// <summary>
    /// a Chain of Responsibility
    /// to Invoke commands (Invoker of the Command Pattern)
    /// to Execute commands by the Actions stored in the ReceiverActions (Receiver of the Command Pattern)
    /// </summary>
    public class EventControler : ActionBase, IEventControler, IDisposable {

        IDictionary<Type, IAction> _actions = null;
        public IDictionary<Type, IAction> Actions {
            get {
                if (_actions == null) {
                    _actions = new Dictionary<Type, IAction> ();
                }
                return _actions;
            }
        }

        public virtual bool UserEventsDisabled { get; set; }
        // Invokers:
        public List<IMouseAction> MouseActions = new List<IMouseAction>();
        public List<IKeyAction> KeyActions = new List<IKeyAction>();

        // Receivers:
        public List<IReceiver> ReceiverActions = new List<IReceiver>();
        public List<IRenderAction> RenderActions = new List<IRenderAction>();
        

        protected int ActionsSort(IAction x, IAction y) {
            return x.Priority.CompareTo(y.Priority);
        }

        public virtual void Add(IAction action) {
            Actions.Add(action.GetType(), action);
            
            if (action is IMouseAction) {
                MouseActions.Add((IMouseAction)action);
                MouseActions.Sort(ActionsSort);
            }

            if (action is IRenderAction) {
                RenderActions.Add((IRenderAction)action);
                RenderActions.Sort(ActionsSort);
            }


            if (action is IKeyAction) {
                KeyActions.Add((IKeyAction)action);
                KeyActions.Sort(ActionsSort);
            }

            if (action is IReceiver) {
                ReceiverActions.Add((IReceiver)action);
                ReceiverActions.Sort(ActionsSort);
            }
        }

        public virtual void Remove(IAction action) {
            if (action == null)
                return; 

            Actions.Remove(action.GetType());
            
            if (action is IMouseAction) {
                MouseActions.Remove((IMouseAction)action);
            }

            if (action is IRenderAction) {
                RenderActions.Remove((IRenderAction)action);
            }

            if (action is IKeyAction) {
                KeyActions.Remove((IKeyAction)action);
            }

            if (action is IReceiver) {
                ReceiverActions.Remove((IReceiver)action);
            }
        }

        public virtual void Add<T>(T value, ref T action) where T : class, IAction {
            if ((action != null) && (action != value)) {
                Remove(action);
                action.Dispose();

            }
            action = value;
            if (action != null) {
                Add(action);
            }

        }

        public virtual T GetAction<T>() {
            IAction result = null;
            if (Actions.TryGetValue(typeof(T), out result))
                return (T)result;
            else { // try harder:
                T r = default(T);
                foreach (var action in Actions) {
                    if (action.Value is T) {
                        r = (T) action.Value;
                        break;
                    }
                }
                return r;
            }
            
        }

        #region IDisposable Member

        public override void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IMouseAction Member

        public void OnMouseDown(MouseActionEventArgs e) {
            Resolved = false;
            if (UserEventsDisabled)
                return;
            foreach (IMouseAction mouseAction in MouseActions) {
                if (mouseAction.Enabled) {
                    mouseAction.OnMouseDown(e);
                    Resolved = mouseAction.Resolved || Resolved;
                    if (mouseAction.Exclusive) {
                        break;
                    }
                    //if (mouseAction is IDragDropAction &&
                    //    !( (IDragDropAction)mouseAction ).Dragging) {
                    //    break;
                    //}
                }
            }
            Execute();
        }

        public void OnMouseMove(MouseActionEventArgs e) {
            Resolved = false;
            if (UserEventsDisabled)
                return;
            foreach (IMouseAction mouseAction in MouseActions) {
                if (mouseAction.Enabled) {
                    mouseAction.OnMouseMove(e);
                    Resolved = mouseAction.Resolved || Resolved;
                    if (mouseAction.Exclusive) {
                        break;
                    }
                }
            }
            Execute();
        }

        public void OnMouseHover(MouseActionEventArgs e) {
            if (UserEventsDisabled)
                return;
            foreach (IMouseAction mouseAction in MouseActions) {
                if (mouseAction.Enabled) {
                    mouseAction.OnMouseHover(e);
                }
            }
            Execute();
        }

        public void OnMouseUp(MouseActionEventArgs e) {
            if (UserEventsDisabled)
                return;
            Resolved = false;
            foreach (IMouseAction mouseAction in MouseActions) {
                if (mouseAction.Enabled) {
                    Resolved = mouseAction.Resolved || Resolved;
                    bool exclusive = mouseAction.Exclusive;
                    mouseAction.OnMouseUp(e);
                    if (exclusive || mouseAction.Exclusive) {
                        break;
                    }
                }
            }
            Execute();
        }

        #endregion

        #region IKeyAction Member

        public void OnKeyDown(KeyActionEventArgs e) {
            if (UserEventsDisabled)
                return;
            Resolved = false;
            foreach (IKeyAction keyAction in KeyActions) {
                if (keyAction.Enabled) {
                    keyAction.OnKeyDown(e);
                    Resolved = keyAction.Resolved || Resolved;
                    if (keyAction.Exclusive) {
                        break;
                    }
                }
            }
            Execute();
        }

        public void OnKeyPress(KeyActionPressEventArgs e) {
            if (UserEventsDisabled)
                return;
            Resolved = false;
            foreach (IKeyAction keyAction in KeyActions) {
                if (keyAction.Enabled) {
                    keyAction.OnKeyPress(e);
                    Resolved = keyAction.Resolved || Resolved;
                    if (keyAction.Exclusive || e.Handled) {
                        break;
                    }
                }
            }
            Execute();
        }

        public void OnKeyUp(KeyActionEventArgs e) {
            Resolved = false;
            foreach (IKeyAction keyAction in KeyActions) {
                if (keyAction.Enabled) {
                    Resolved = keyAction.Resolved || Resolved;
                    bool exclusive = keyAction.Exclusive;
                    keyAction.OnKeyUp(e);
                    if (exclusive || e.Handled) {
                        break;
                    }
                }
            }
            Execute();
        }

        #endregion

        #region IRenderAction Member

        public void OnPaint(IRenderEventArgs e) {
            foreach (IRenderAction renderAction in RenderActions) {
                if (renderAction.Enabled)
                    renderAction.OnPaint(e);
            }
        }

        #endregion

        #region IReceiver Member
        public void Execute() {
            foreach (IReceiver action in ReceiverActions) {
                if (action.Enabled)
                    action.Execute();
            }
            Done();
        }

        public void Done() {
            foreach (IReceiver action in ReceiverActions) {
                if (action.Enabled)
                    action.Done();
            }
        }

        public void Invoke() {
            foreach (IReceiver action in ReceiverActions) {
                if (action.Enabled)
                    action.Invoke();
            }
            Execute();
        }

        #endregion

    }
}