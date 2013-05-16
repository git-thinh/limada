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
 * http://www.limada.org
 * 
 */

using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using System.Diagnostics;

namespace Limaki.Data.db4o {
    public class Gateway : GatewayBase {
        # region session

        private IEmbeddedConfiguration _configuration = null;
        public ICommonConfiguration Configuration {
            get {
                if (_configuration == null) {
                    _configuration = Db4oEmbedded.NewConfiguration();
                    InitConfiguration(_configuration.Common);
                }
                return _configuration.Common;
            }
        }

        bool _isClosed = false;

        IObjectContainer _session = null;
        public virtual IObjectContainer Session {
            get {
                if (!_isClosed) {
                    if (_session == null) {
                        try {
                            _session = CreateSession(_configuration);

                        } catch (Exception e) {
                            Exception ex = new Exception(
                                e.Message + "\nFile open failed:\t" +
                                IoInfo.Path + IoInfo.Name + this.FileExtension,
                                e);
                            throw ex;
                        }
                    }
                }
                return _session;
            }
        }

        public virtual IObjectContainer CreateSession(IEmbeddedConfiguration config) {
            var file = this.IoInfo.Path + this.IoInfo.Name +
                       this.FileExtension;
            if (!System.IO.File.Exists(file)) {
                config.File.BlockSize = 16;
                Trace.TraceInformation("{0}: File not exists: {1}", this.GetType().FullName, file);
            }
            return Db4oEmbedded.OpenFile(config,file);
        }

        # endregion session

        #region IGateway Member

        public override void Open(IoInfo ioInfo) {
            _isClosed = false;
            this.IoInfo = ioInfo;
        }

        public override void Close() {
            _isClosed = true;
            if (_session != null) {
                try {
                    Session.Close();
                    Session.Dispose();
                    _configuration = null;
                } catch (Db4objects.Db4o.Ext.Db4oException e) {
                    // TODO: a curios exception is thrown here:
                    // "This functionality is only available for indexed fields."
                    // it is: failing of UniqueFieldValueConstraint
                    // see also: Graph.Flush()
                    throw e;
                } finally {
                    _session = null;
                    _configuration = null;
                }
            }
            this.IoInfo = null;
        }

        public virtual bool HasSession() {
            return _session != null;
        }
        public override bool IsOpen() {
            return IoInfo != null;
        }
        public override bool IsClosed() {
            return _isClosed;
        }
        public virtual void InitConfiguration(ICommonConfiguration configuration) {
            configuration.MarkTransient(typeof(Limaki.Common.TransientAttribute).FullName);
        }

        public override string FileExtension {
            get { return ".limo"; }
        }

        #endregion
    }
}
