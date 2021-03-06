﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.2032
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

namespace WebMeeting.Client.Xml_Handling {
    using System;
    using System.Data;
    using System.Xml;
    using System.Runtime.Serialization;
    
    
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.ToolboxItem(true)]
    public class docsSharing : DataSet {
        
        private DocsSharedDataTable tableDocsShared;
        
        public docsSharing() {
            this.InitClass();
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        protected docsSharing(SerializationInfo info, StreamingContext context) {
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((strSchema != null)) {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(new XmlTextReader(new System.IO.StringReader(strSchema)));
                if ((ds.Tables["DocsShared"] != null)) {
                    this.Tables.Add(new DocsSharedDataTable(ds.Tables["DocsShared"]));
                }
                this.DataSetName = ds.DataSetName;
                this.Prefix = ds.Prefix;
                this.Namespace = ds.Namespace;
                this.Locale = ds.Locale;
                this.CaseSensitive = ds.CaseSensitive;
                this.EnforceConstraints = ds.EnforceConstraints;
                this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
                this.InitVars();
            }
            else {
                this.InitClass();
            }
            this.GetSerializationData(info, context);
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public DocsSharedDataTable DocsShared {
            get {
                return this.tableDocsShared;
            }
        }
        
        public override DataSet Clone() {
            docsSharing cln = ((docsSharing)(base.Clone()));
            cln.InitVars();
            return cln;
        }
        
        protected override bool ShouldSerializeTables() {
            return false;
        }
        
        protected override bool ShouldSerializeRelations() {
            return false;
        }
        
        protected override void ReadXmlSerializable(XmlReader reader) {
            this.Reset();
            DataSet ds = new DataSet();
            ds.ReadXml(reader);
            if ((ds.Tables["DocsShared"] != null)) {
                this.Tables.Add(new DocsSharedDataTable(ds.Tables["DocsShared"]));
            }
            this.DataSetName = ds.DataSetName;
            this.Prefix = ds.Prefix;
            this.Namespace = ds.Namespace;
            this.Locale = ds.Locale;
            this.CaseSensitive = ds.CaseSensitive;
            this.EnforceConstraints = ds.EnforceConstraints;
            this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
            this.InitVars();
        }
        
        protected override System.Xml.Schema.XmlSchema GetSchemaSerializable() {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            this.WriteXmlSchema(new XmlTextWriter(stream, null));
            stream.Position = 0;
            return System.Xml.Schema.XmlSchema.Read(new XmlTextReader(stream), null);
        }
        
        internal void InitVars() {
            this.tableDocsShared = ((DocsSharedDataTable)(this.Tables["DocsShared"]));
            if ((this.tableDocsShared != null)) {
                this.tableDocsShared.InitVars();
            }
        }
        
        private void InitClass() {
            this.DataSetName = "docsSharing";
            this.Prefix = "";
            this.Namespace = "http://tempuri.org/docsSharing.xsd";
            this.Locale = new System.Globalization.CultureInfo("en-US");
            this.CaseSensitive = false;
            this.EnforceConstraints = true;
            this.tableDocsShared = new DocsSharedDataTable();
            this.Tables.Add(this.tableDocsShared);
        }
        
        private bool ShouldSerializeDocsShared() {
            return false;
        }
        
        private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) {
            if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) {
                this.InitVars();
            }
        }
        
        public delegate void DocsSharedRowChangeEventHandler(object sender, DocsSharedRowChangeEvent e);
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class DocsSharedDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnID;
            
            private DataColumn columnRemoteURL;
            
            private DataColumn columnLocalPath;
            
            private DataColumn columnTotalPages;
            
            private DataColumn columnDocType;
            
            private DataColumn columnFileName;
            
            private DataColumn columnSession;
            
            private DataColumn columnLastAccessDate;
            
            internal DocsSharedDataTable() : 
                    base("DocsShared") {
                this.InitClass();
            }
            
            internal DocsSharedDataTable(DataTable table) : 
                    base(table.TableName) {
                if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
                    this.CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
                    this.Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace)) {
                    this.Namespace = table.Namespace;
                }
                this.Prefix = table.Prefix;
                this.MinimumCapacity = table.MinimumCapacity;
                this.DisplayExpression = table.DisplayExpression;
            }
            
            [System.ComponentModel.Browsable(false)]
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            internal DataColumn IDColumn {
                get {
                    return this.columnID;
                }
            }
            
            internal DataColumn RemoteURLColumn {
                get {
                    return this.columnRemoteURL;
                }
            }
            
            internal DataColumn LocalPathColumn {
                get {
                    return this.columnLocalPath;
                }
            }
            
            internal DataColumn TotalPagesColumn {
                get {
                    return this.columnTotalPages;
                }
            }
            
            internal DataColumn DocTypeColumn {
                get {
                    return this.columnDocType;
                }
            }
            
            internal DataColumn FileNameColumn {
                get {
                    return this.columnFileName;
                }
            }
            
            internal DataColumn SessionColumn {
                get {
                    return this.columnSession;
                }
            }
            
            internal DataColumn LastAccessDateColumn {
                get {
                    return this.columnLastAccessDate;
                }
            }
            
            public DocsSharedRow this[int index] {
                get {
                    return ((DocsSharedRow)(this.Rows[index]));
                }
            }
            
            public event DocsSharedRowChangeEventHandler DocsSharedRowChanged;
            
            public event DocsSharedRowChangeEventHandler DocsSharedRowChanging;
            
            public event DocsSharedRowChangeEventHandler DocsSharedRowDeleted;
            
            public event DocsSharedRowChangeEventHandler DocsSharedRowDeleting;
            
            public void AddDocsSharedRow(DocsSharedRow row) {
                this.Rows.Add(row);
            }
            
            public DocsSharedRow AddDocsSharedRow(string RemoteURL, string LocalPath, int TotalPages, string DocType, string FileName, int Session, string LastAccessDate) {
                DocsSharedRow rowDocsSharedRow = ((DocsSharedRow)(this.NewRow()));
                rowDocsSharedRow.ItemArray = new object[] {
                        null,
                        RemoteURL,
                        LocalPath,
                        TotalPages,
                        DocType,
                        FileName,
                        Session,
                        LastAccessDate};
                this.Rows.Add(rowDocsSharedRow);
                return rowDocsSharedRow;
            }
            
            public DocsSharedRow FindByID(int ID) {
                return ((DocsSharedRow)(this.Rows.Find(new object[] {
                            ID})));
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                DocsSharedDataTable cln = ((DocsSharedDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new DocsSharedDataTable();
            }
            
            internal void InitVars() {
                this.columnID = this.Columns["ID"];
                this.columnRemoteURL = this.Columns["RemoteURL"];
                this.columnLocalPath = this.Columns["LocalPath"];
                this.columnTotalPages = this.Columns["TotalPages"];
                this.columnDocType = this.Columns["DocType"];
                this.columnFileName = this.Columns["FileName"];
                this.columnSession = this.Columns["Session"];
                this.columnLastAccessDate = this.Columns["LastAccessDate"];
            }
            
            private void InitClass() {
                this.columnID = new DataColumn("ID", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnID);
                this.columnRemoteURL = new DataColumn("RemoteURL", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnRemoteURL);
                this.columnLocalPath = new DataColumn("LocalPath", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnLocalPath);
                this.columnTotalPages = new DataColumn("TotalPages", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnTotalPages);
                this.columnDocType = new DataColumn("DocType", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnDocType);
                this.columnFileName = new DataColumn("FileName", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnFileName);
                this.columnSession = new DataColumn("Session", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnSession);
                this.columnLastAccessDate = new DataColumn("LastAccessDate", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnLastAccessDate);
                this.Constraints.Add(new UniqueConstraint("docsSharingKey1", new DataColumn[] {
                                this.columnID}, true));
                this.columnID.AutoIncrement = true;
                this.columnID.AllowDBNull = false;
                this.columnID.Unique = true;
            }
            
            public DocsSharedRow NewDocsSharedRow() {
                return ((DocsSharedRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new DocsSharedRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(DocsSharedRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.DocsSharedRowChanged != null)) {
                    this.DocsSharedRowChanged(this, new DocsSharedRowChangeEvent(((DocsSharedRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.DocsSharedRowChanging != null)) {
                    this.DocsSharedRowChanging(this, new DocsSharedRowChangeEvent(((DocsSharedRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.DocsSharedRowDeleted != null)) {
                    this.DocsSharedRowDeleted(this, new DocsSharedRowChangeEvent(((DocsSharedRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.DocsSharedRowDeleting != null)) {
                    this.DocsSharedRowDeleting(this, new DocsSharedRowChangeEvent(((DocsSharedRow)(e.Row)), e.Action));
                }
            }
            
            public void RemoveDocsSharedRow(DocsSharedRow row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class DocsSharedRow : DataRow {
            
            private DocsSharedDataTable tableDocsShared;
            
            internal DocsSharedRow(DataRowBuilder rb) : 
                    base(rb) {
                this.tableDocsShared = ((DocsSharedDataTable)(this.Table));
            }
            
            public int ID {
                get {
                    return ((int)(this[this.tableDocsShared.IDColumn]));
                }
                set {
                    this[this.tableDocsShared.IDColumn] = value;
                }
            }
            
            public string RemoteURL {
                get {
                    try {
                        return ((string)(this[this.tableDocsShared.RemoteURLColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableDocsShared.RemoteURLColumn] = value;
                }
            }
            
            public string LocalPath {
                get {
                    try {
                        return ((string)(this[this.tableDocsShared.LocalPathColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableDocsShared.LocalPathColumn] = value;
                }
            }
            
            public int TotalPages {
                get {
                    try {
                        return ((int)(this[this.tableDocsShared.TotalPagesColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableDocsShared.TotalPagesColumn] = value;
                }
            }
            
            public string DocType {
                get {
                    try {
                        return ((string)(this[this.tableDocsShared.DocTypeColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableDocsShared.DocTypeColumn] = value;
                }
            }
            
            public string FileName {
                get {
                    try {
                        return ((string)(this[this.tableDocsShared.FileNameColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableDocsShared.FileNameColumn] = value;
                }
            }
            
            public int Session {
                get {
                    try {
                        return ((int)(this[this.tableDocsShared.SessionColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableDocsShared.SessionColumn] = value;
                }
            }
            
            public string LastAccessDate {
                get {
                    try {
                        return ((string)(this[this.tableDocsShared.LastAccessDateColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableDocsShared.LastAccessDateColumn] = value;
                }
            }
            
            public bool IsRemoteURLNull() {
                return this.IsNull(this.tableDocsShared.RemoteURLColumn);
            }
            
            public void SetRemoteURLNull() {
                this[this.tableDocsShared.RemoteURLColumn] = System.Convert.DBNull;
            }
            
            public bool IsLocalPathNull() {
                return this.IsNull(this.tableDocsShared.LocalPathColumn);
            }
            
            public void SetLocalPathNull() {
                this[this.tableDocsShared.LocalPathColumn] = System.Convert.DBNull;
            }
            
            public bool IsTotalPagesNull() {
                return this.IsNull(this.tableDocsShared.TotalPagesColumn);
            }
            
            public void SetTotalPagesNull() {
                this[this.tableDocsShared.TotalPagesColumn] = System.Convert.DBNull;
            }
            
            public bool IsDocTypeNull() {
                return this.IsNull(this.tableDocsShared.DocTypeColumn);
            }
            
            public void SetDocTypeNull() {
                this[this.tableDocsShared.DocTypeColumn] = System.Convert.DBNull;
            }
            
            public bool IsFileNameNull() {
                return this.IsNull(this.tableDocsShared.FileNameColumn);
            }
            
            public void SetFileNameNull() {
                this[this.tableDocsShared.FileNameColumn] = System.Convert.DBNull;
            }
            
            public bool IsSessionNull() {
                return this.IsNull(this.tableDocsShared.SessionColumn);
            }
            
            public void SetSessionNull() {
                this[this.tableDocsShared.SessionColumn] = System.Convert.DBNull;
            }
            
            public bool IsLastAccessDateNull() {
                return this.IsNull(this.tableDocsShared.LastAccessDateColumn);
            }
            
            public void SetLastAccessDateNull() {
                this[this.tableDocsShared.LastAccessDateColumn] = System.Convert.DBNull;
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class DocsSharedRowChangeEvent : EventArgs {
            
            private DocsSharedRow eventRow;
            
            private DataRowAction eventAction;
            
            public DocsSharedRowChangeEvent(DocsSharedRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public DocsSharedRow Row {
                get {
                    return this.eventRow;
                }
            }
            
            public DataRowAction Action {
                get {
                    return this.eventAction;
                }
            }
        }
    }
}
