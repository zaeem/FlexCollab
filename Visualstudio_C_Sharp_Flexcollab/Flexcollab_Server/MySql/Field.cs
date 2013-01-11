namespace MySQL
{
using System.Runtime.InteropServices;

///<remarks>
///<para>
/// MySql P/Invoke implementation test program
/// Brad Merrill
/// 3-Mar-2002
///</para>
///<para>
/// This structure contains information about a field, such as the
/// field's name, type, and size. Its members are described in more
/// detail below.  You may obtain the <see cref="Field"/> structures for
/// each field by calling
/// <see cref="MySql.FetchField"/>
/// repeatedly.
/// Field values are not part of this structure;
/// they are contained in a Row structure.
///</para>
///</remarks>
[StructLayout(LayoutKind.Sequential)]
public class Field
  {
  ///<value>name of column</value>
  [MarshalAs(UnmanagedType.LPStr)]
    public string Name;
  ///<value>table of column</value>
  [MarshalAs(UnmanagedType.LPStr)]
    public string Table;
  ///<value>default value</value>
  [MarshalAs(UnmanagedType.LPStr)]
    public string Def;
  ///<value>type of field</value>
  public int FieldTypes;
  ///<value>width of column</value>
  public uint Length;
  ///<value>max width of selected set</value>
  public uint MaxLength;
  ///<value>div flags</value>
  public uint Flags;
  ///<value>number of decimals in field</value>
  public uint Decimals;	
  }
}
