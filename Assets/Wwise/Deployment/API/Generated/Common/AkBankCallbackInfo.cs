#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 2.0.11
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */


using System;
using System.Runtime.InteropServices;

public class AkBankCallbackInfo : IDisposable {
  private IntPtr swigCPtr;
  protected bool swigCMemOwn;

  internal AkBankCallbackInfo(IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = cPtr;
  }

  internal static IntPtr getCPtr(AkBankCallbackInfo obj) {
    return (obj == null) ? IntPtr.Zero : obj.swigCPtr;
  }

  ~AkBankCallbackInfo() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr != IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          AkSoundEnginePINVOKE.CSharp_delete_AkBankCallbackInfo(swigCPtr);
        }
        swigCPtr = IntPtr.Zero;
      }
      GC.SuppressFinalize(this);
    }
  }

  public uint bankID {
    get {
      uint ret = AkSoundEnginePINVOKE.CSharp_AkBankCallbackInfo_bankID_get(swigCPtr);

      return ret;
    } 
  }

  public IntPtr inMemoryBankPtr { get { return AkSoundEnginePINVOKE.CSharp_AkBankCallbackInfo_inMemoryBankPtr_get(swigCPtr);
 }
  }

  public AKRESULT loadResult {
    get {
      AKRESULT ret = (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkBankCallbackInfo_loadResult_get(swigCPtr);

      return ret;
    } 
  }

  public int memPoolId {
    get {
      int ret = AkSoundEnginePINVOKE.CSharp_AkBankCallbackInfo_memPoolId_get(swigCPtr);

      return ret;
    } 
  }

  public AkBankCallbackInfo() : this(AkSoundEnginePINVOKE.CSharp_new_AkBankCallbackInfo(), true) {

  }

}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.