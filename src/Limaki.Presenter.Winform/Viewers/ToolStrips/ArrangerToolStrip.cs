﻿/*
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

using System.Windows.Forms;
using Limaki.Common;
using Limaki.Drawing;
using System.ComponentModel;
using System;
using Limaki.Presenter.Layout;
using Limaki.UseCases.Viewers.ToolStripViewers;
using Alignment = Xwt.Alignment;

namespace Limaki.UseCases.Winform.Viewers.ToolStripViewers {
    

    public partial class ArrangerToolStrip : ToolStrip, IArrangerTool {

        ArrangerToolController _controller = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ArrangerToolController Controller {
            get { return _controller ?? (_controller = new ArrangerToolController { Tool = this }); }
        }

        public ArrangerToolStrip() {
            InitializeComponent();
            Compose();
            Controller.Tool = this;
        }
        
        protected virtual void Compose() {
            var options = new AllignerOptions();
            options.Dimension = Dimension.Y;

            var size = new System.Drawing.Size(36, 36);
            Action action = () => Columns(options);

            var logicalLayout = new ToolStripCommand {
                Action = (s) => {
                    action = () => LogicalLayout(options);
                    action();
                },
                Image = Limaki.Presenter.Properties.Resources.LogicalLayout,
                Size = size,
            };

            var fullLayout = new ToolStripCommand {
                Action = (s) => {
                    action = () => FullLayout(options);
                    action();
                },
                Image = Limaki.Presenter.Properties.Resources.ModifyLayout24,
                Size = size,
            };

            var columns = new ToolStripCommand {
                Action = (s) => {
                    action = () => Columns(options);
                    action();
                },
                Image = Limaki.Presenter.Properties.Resources.ArrageRows,
                Size = size,
            };
            var oneColumn = new ToolStripCommand {
                Action = (s) => {
                    action = () => OneColumn(options);
                    action();
                },
                Image = Limaki.Presenter.Properties.Resources.ArrangeOneRow,
                Size = size,
            };
            var arrangeLeft = new ToolStripCommand {
                Action = (s) => {
                    options.AlignX = Alignment.Start;
                    action();
                },
                Image = Limaki.Presenter.Properties.Resources.ArrangeLeft,
                Size = size,
            };
            var arrangeCenter = new ToolStripCommand {
                Action = (s) => {
                    options.AlignX = Alignment.Center;
                    action();
                },
                Image = Limaki.Presenter.Properties.Resources.ArrangeCenter,
                Size = size,
            };
            var arrangeRight = new ToolStripCommand {
                Action = (s) => {
                    options.AlignX = Alignment.End;
                    action();
                },
                Image = Limaki.Presenter.Properties.Resources.ArrangeRight,
                Size = size,
            };

            var arrangeTop = new ToolStripCommand {
                Action = (s) => {
                    options.AlignY = Alignment.Start;
                    action();
                },
                Image = Limaki.Presenter.Properties.Resources.ArrangeTop,
                Size = size,
            };
            var arrangeCenterV = new ToolStripCommand {
                Action = (s) => {
                    options.AlignY = Alignment.Center;
                    action();
                },
                Image = Limaki.Presenter.Properties.Resources.ArrangeMiddle,
                Size = size,
            };
            var arrangeBottom = new ToolStripCommand {
                Action = (s) => {
                    options.AlignY = Alignment.End;
                    action();
                },
                Image = Limaki.Presenter.Properties.Resources.ArrangeBottom,
                Size = size,
            };

            var undo = new ToolStripCommand {
                Action = (s) => Undo(),
                Size = size,
                Image = Limaki.Presenter.Properties.Resources.Undo,
            };
            var horizontalButton = new ToolStripDropDownButtonEx {Command = arrangeLeft};
            horizontalButton.DropDownItems.AddRange(new ToolStripItem[] {
                  new ToolStripMenuItemEx {Command=arrangeCenter,ToggleOnClick=horizontalButton},
                  new ToolStripMenuItemEx {Command=arrangeRight,ToggleOnClick=horizontalButton},
            });
            var verticalButton = new ToolStripDropDownButtonEx { Command = arrangeTop };
            verticalButton.DropDownItems.AddRange(new ToolStripItem[] {
                  new ToolStripMenuItemEx {Command=arrangeCenterV,ToggleOnClick=verticalButton},
                  new ToolStripMenuItemEx {Command=arrangeBottom,ToggleOnClick=verticalButton},
            });

            var layoutButton = new ToolStripDropDownButtonEx { Command = logicalLayout };
            layoutButton.DropDownItems.AddRange(new ToolStripItem[] {
                new ToolStripMenuItemEx {Command=columns,ToggleOnClick=layoutButton},    
                new ToolStripMenuItemEx {Command=oneColumn,ToggleOnClick=layoutButton},          
                new ToolStripMenuItemEx {Command=fullLayout}, 
            });

            this.Items.AddRange(new ToolStripItem[] {
                layoutButton,
                horizontalButton,
                verticalButton,
                new ToolStripButtonEx {Command=undo},
            });
        }

        public virtual void Undo() {
            Controller.Undo();
        }
      
        public virtual void Columns(AllignerOptions options) {
            Controller.Columns(options);
        }
        public virtual void OneColumn(AllignerOptions options) {
            Controller.OneColumn(options);
        }
        public virtual void LogicalLayout(AllignerOptions options) {
            Controller.LogicalLayout(options);
        }
        public virtual void FullLayout(AllignerOptions options) {
            Controller.FullLayout(options);
        }
    }
}