/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008 - 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.IO;
using System.Windows.Forms;
using Gecko;

namespace Limaki.Swf.Backends {

    public class XulRunner {

        public string XulDir (string basedir) {
            var xulrunner = "xulrunner18.0-" + (OS.IsWin64Process ? "64" : "32");
            foreach (var dir in new string[] { @"Plugins\", @"..\3rdParty\bin\" }) {
                var s = dir;
                for (int i = 0; i <= 10; i++) {
                    var xuldir = basedir + s + xulrunner;
                    if (Directory.Exists(xuldir))
                        return xuldir;
                    s = @"..\" + s;
                }
            }
            return null;
        }

        protected static bool Running = false;
        public bool Initialize() {
            if(!Running) {
                string xulDir = XulDir(AppDomain.CurrentDomain.BaseDirectory);
                if (xulDir == null)
                    throw new ArgumentException("xulrunner is missing");
                Xpcom.Initialize(xulDir);
                // this makes troubles:
                //Application.ApplicationExit += (sender, e) => {
                //    Xpcom.Shutdown();
                //};
                Running = true;
            }
            return Running;
        }
    }
}