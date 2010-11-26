/*
 * Limaki 
 * Version 0.07
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */


using System;
using System.Collections.Generic;
using System.Text;

namespace Limaki.Graphs {
    public interface IEdge<T> {
        T Root { get;set;}
        T Leaf { get;set;}
    }

    public interface IEdge:IEdge<object>{};

}
