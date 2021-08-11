// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace YouYou.DataTable
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct DTItem : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static DTItem GetRootAsDTItem(ByteBuffer _bb) { return GetRootAsDTItem(_bb, new DTItem()); }
  public static DTItem GetRootAsDTItem(ByteBuffer _bb, DTItem obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public DTItem __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public int Id { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public string Name { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetNameBytes() { return __p.__vector_as_span<byte>(6, 1); }
#else
  public ArraySegment<byte>? GetNameBytes() { return __p.__vector_as_arraysegment(6); }
#endif
  public byte[] GetNameArray() { return __p.__vector_as_array<byte>(6); }
  public int Type { get { int o = __p.__offset(8); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int UsedLevel { get { int o = __p.__offset(10); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public string UsedMethod { get { int o = __p.__offset(12); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetUsedMethodBytes() { return __p.__vector_as_span<byte>(12, 1); }
#else
  public ArraySegment<byte>? GetUsedMethodBytes() { return __p.__vector_as_arraysegment(12); }
#endif
  public byte[] GetUsedMethodArray() { return __p.__vector_as_array<byte>(12); }
  public int SellMoney { get { int o = __p.__offset(14); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int Quality { get { int o = __p.__offset(16); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public string Description { get { int o = __p.__offset(18); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetDescriptionBytes() { return __p.__vector_as_span<byte>(18, 1); }
#else
  public ArraySegment<byte>? GetDescriptionBytes() { return __p.__vector_as_arraysegment(18); }
#endif
  public byte[] GetDescriptionArray() { return __p.__vector_as_array<byte>(18); }
  public string UsedItems { get { int o = __p.__offset(20); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetUsedItemsBytes() { return __p.__vector_as_span<byte>(20, 1); }
#else
  public ArraySegment<byte>? GetUsedItemsBytes() { return __p.__vector_as_arraysegment(20); }
#endif
  public byte[] GetUsedItemsArray() { return __p.__vector_as_array<byte>(20); }
  public int MaxAmount { get { int o = __p.__offset(22); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int PackSort { get { int o = __p.__offset(24); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }

  public static Offset<YouYou.DataTable.DTItem> CreateDTItem(FlatBufferBuilder builder,
      int Id = 0,
      StringOffset NameOffset = default(StringOffset),
      int Type = 0,
      int UsedLevel = 0,
      StringOffset UsedMethodOffset = default(StringOffset),
      int SellMoney = 0,
      int Quality = 0,
      StringOffset DescriptionOffset = default(StringOffset),
      StringOffset UsedItemsOffset = default(StringOffset),
      int maxAmount = 0,
      int packSort = 0) {
    builder.StartTable(11);
    DTItem.AddPackSort(builder, packSort);
    DTItem.AddMaxAmount(builder, maxAmount);
    DTItem.AddUsedItems(builder, UsedItemsOffset);
    DTItem.AddDescription(builder, DescriptionOffset);
    DTItem.AddQuality(builder, Quality);
    DTItem.AddSellMoney(builder, SellMoney);
    DTItem.AddUsedMethod(builder, UsedMethodOffset);
    DTItem.AddUsedLevel(builder, UsedLevel);
    DTItem.AddType(builder, Type);
    DTItem.AddName(builder, NameOffset);
    DTItem.AddId(builder, Id);
    return DTItem.EndDTItem(builder);
  }

  public static void StartDTItem(FlatBufferBuilder builder) { builder.StartTable(11); }
  public static void AddId(FlatBufferBuilder builder, int Id) { builder.AddInt(0, Id, 0); }
  public static void AddName(FlatBufferBuilder builder, StringOffset NameOffset) { builder.AddOffset(1, NameOffset.Value, 0); }
  public static void AddType(FlatBufferBuilder builder, int Type) { builder.AddInt(2, Type, 0); }
  public static void AddUsedLevel(FlatBufferBuilder builder, int UsedLevel) { builder.AddInt(3, UsedLevel, 0); }
  public static void AddUsedMethod(FlatBufferBuilder builder, StringOffset UsedMethodOffset) { builder.AddOffset(4, UsedMethodOffset.Value, 0); }
  public static void AddSellMoney(FlatBufferBuilder builder, int SellMoney) { builder.AddInt(5, SellMoney, 0); }
  public static void AddQuality(FlatBufferBuilder builder, int Quality) { builder.AddInt(6, Quality, 0); }
  public static void AddDescription(FlatBufferBuilder builder, StringOffset DescriptionOffset) { builder.AddOffset(7, DescriptionOffset.Value, 0); }
  public static void AddUsedItems(FlatBufferBuilder builder, StringOffset UsedItemsOffset) { builder.AddOffset(8, UsedItemsOffset.Value, 0); }
  public static void AddMaxAmount(FlatBufferBuilder builder, int maxAmount) { builder.AddInt(9, maxAmount, 0); }
  public static void AddPackSort(FlatBufferBuilder builder, int packSort) { builder.AddInt(10, packSort, 0); }
  public static Offset<YouYou.DataTable.DTItem> EndDTItem(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<YouYou.DataTable.DTItem>(o);
  }
};


}
