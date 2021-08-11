// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace YouYou.DataTable
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct DTChapter : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static DTChapter GetRootAsDTChapter(ByteBuffer _bb) { return GetRootAsDTChapter(_bb, new DTChapter()); }
  public static DTChapter GetRootAsDTChapter(ByteBuffer _bb, DTChapter obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public DTChapter __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public int Id { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public string ChapterName { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetChapterNameBytes() { return __p.__vector_as_span<byte>(6, 1); }
#else
  public ArraySegment<byte>? GetChapterNameBytes() { return __p.__vector_as_arraysegment(6); }
#endif
  public byte[] GetChapterNameArray() { return __p.__vector_as_array<byte>(6); }
  public int GameLevelCount { get { int o = __p.__offset(8); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public string BGPic { get { int o = __p.__offset(10); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetBGPicBytes() { return __p.__vector_as_span<byte>(10, 1); }
#else
  public ArraySegment<byte>? GetBGPicBytes() { return __p.__vector_as_arraysegment(10); }
#endif
  public byte[] GetBGPicArray() { return __p.__vector_as_array<byte>(10); }
  public int BranchLevelId(int j) { int o = __p.__offset(12); return o != 0 ? __p.bb.GetInt(__p.__vector(o) + j * 4) : (int)0; }
  public int BranchLevelIdLength { get { int o = __p.__offset(12); return o != 0 ? __p.__vector_len(o) : 0; } }
#if ENABLE_SPAN_T
  public Span<int> GetBranchLevelIdBytes() { return __p.__vector_as_span<int>(12, 4); }
#else
  public ArraySegment<byte>? GetBranchLevelIdBytes() { return __p.__vector_as_arraysegment(12); }
#endif
  public int[] GetBranchLevelIdArray() { return __p.__vector_as_array<int>(12); }
  public string BranchLevelName(int j) { int o = __p.__offset(14); return o != 0 ? __p.__string(__p.__vector(o) + j * 4) : null; }
  public int BranchLevelNameLength { get { int o = __p.__offset(14); return o != 0 ? __p.__vector_len(o) : 0; } }
  public float Uvx { get { int o = __p.__offset(16); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }
  public float Uvy { get { int o = __p.__offset(18); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }

  public static Offset<YouYou.DataTable.DTChapter> CreateDTChapter(FlatBufferBuilder builder,
      int Id = 0,
      StringOffset ChapterNameOffset = default(StringOffset),
      int GameLevelCount = 0,
      StringOffset BG_PicOffset = default(StringOffset),
      VectorOffset BranchLevelIdOffset = default(VectorOffset),
      VectorOffset BranchLevelNameOffset = default(VectorOffset),
      float Uvx = 0.0f,
      float Uvy = 0.0f) {
    builder.StartTable(8);
    DTChapter.AddUvy(builder, Uvy);
    DTChapter.AddUvx(builder, Uvx);
    DTChapter.AddBranchLevelName(builder, BranchLevelNameOffset);
    DTChapter.AddBranchLevelId(builder, BranchLevelIdOffset);
    DTChapter.AddBGPic(builder, BG_PicOffset);
    DTChapter.AddGameLevelCount(builder, GameLevelCount);
    DTChapter.AddChapterName(builder, ChapterNameOffset);
    DTChapter.AddId(builder, Id);
    return DTChapter.EndDTChapter(builder);
  }

  public static void StartDTChapter(FlatBufferBuilder builder) { builder.StartTable(8); }
  public static void AddId(FlatBufferBuilder builder, int Id) { builder.AddInt(0, Id, 0); }
  public static void AddChapterName(FlatBufferBuilder builder, StringOffset ChapterNameOffset) { builder.AddOffset(1, ChapterNameOffset.Value, 0); }
  public static void AddGameLevelCount(FlatBufferBuilder builder, int GameLevelCount) { builder.AddInt(2, GameLevelCount, 0); }
  public static void AddBGPic(FlatBufferBuilder builder, StringOffset BGPicOffset) { builder.AddOffset(3, BGPicOffset.Value, 0); }
  public static void AddBranchLevelId(FlatBufferBuilder builder, VectorOffset BranchLevelIdOffset) { builder.AddOffset(4, BranchLevelIdOffset.Value, 0); }
  public static VectorOffset CreateBranchLevelIdVector(FlatBufferBuilder builder, int[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddInt(data[i]); return builder.EndVector(); }
  public static VectorOffset CreateBranchLevelIdVectorBlock(FlatBufferBuilder builder, int[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static void StartBranchLevelIdVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddBranchLevelName(FlatBufferBuilder builder, VectorOffset BranchLevelNameOffset) { builder.AddOffset(5, BranchLevelNameOffset.Value, 0); }
  public static VectorOffset CreateBranchLevelNameVector(FlatBufferBuilder builder, StringOffset[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static VectorOffset CreateBranchLevelNameVectorBlock(FlatBufferBuilder builder, StringOffset[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static void StartBranchLevelNameVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddUvx(FlatBufferBuilder builder, float Uvx) { builder.AddFloat(6, Uvx, 0.0f); }
  public static void AddUvy(FlatBufferBuilder builder, float Uvy) { builder.AddFloat(7, Uvy, 0.0f); }
  public static Offset<YouYou.DataTable.DTChapter> EndDTChapter(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<YouYou.DataTable.DTChapter>(o);
  }
};


}
