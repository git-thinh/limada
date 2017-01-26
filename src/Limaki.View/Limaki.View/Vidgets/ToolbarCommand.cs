/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2012-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.Vidgets {

    public interface IToolbarCommand {
        Action<object> Action { get; set; }
        Image Image { get; set; }
        string Label { get; set; }
        string ToolTipText { get; set; }
        Size Size { get; set; }
    }

    public class ToolbarCommand : IToolbarCommand {

        public ToolbarCommand () { }

        public ToolbarCommand (IToolbarCommand value) {
            if (value != null) {
                this.Image = value.Image;
                this.Label = value.Label;
                this.ToolTipText = value.ToolTipText;
                this.Size = value.Size;
                this.Action = value.Action;
            }    
        }

        public Action<object> Action { get; set; }
        public Image Image { get; set; }
        public string Label { get; set; }
        public string ToolTipText { get; set; }
        public Size Size { get; set; }
    }
}