#define ICALL_TABLE_corlib 1

static int corlib_icall_indexes [] = {
261,
273,
274,
275,
276,
277,
278,
279,
280,
281,
284,
285,
286,
462,
463,
464,
494,
495,
496,
516,
517,
518,
519,
636,
637,
638,
641,
691,
692,
693,
696,
698,
700,
702,
707,
715,
716,
717,
718,
719,
720,
721,
722,
723,
724,
725,
726,
727,
728,
729,
730,
731,
733,
734,
735,
736,
737,
738,
739,
836,
837,
838,
839,
840,
841,
842,
843,
844,
845,
846,
847,
848,
849,
850,
851,
852,
854,
855,
856,
857,
858,
859,
860,
927,
928,
998,
1005,
1008,
1010,
1015,
1016,
1018,
1019,
1023,
1025,
1026,
1028,
1030,
1031,
1034,
1035,
1036,
1039,
1041,
1044,
1046,
1048,
1057,
1127,
1129,
1131,
1141,
1142,
1143,
1144,
1146,
1153,
1154,
1155,
1156,
1157,
1165,
1166,
1167,
1171,
1172,
1174,
1178,
1179,
1180,
1464,
1666,
1667,
10046,
10047,
10049,
10050,
10051,
10052,
10053,
10054,
10056,
10058,
10060,
10061,
10062,
10073,
10075,
10080,
10082,
10084,
10086,
10138,
10145,
10146,
10148,
10149,
10150,
10151,
10152,
10154,
10156,
10157,
11386,
11390,
11392,
11393,
11394,
11395,
11662,
11663,
11664,
11665,
11685,
11686,
11687,
11689,
11691,
11754,
11839,
11841,
11843,
11853,
11854,
11855,
11856,
11857,
12343,
12344,
12349,
12350,
12389,
12435,
12442,
12449,
12460,
12464,
12490,
12570,
12576,
12578,
12589,
12591,
12592,
12593,
12600,
12615,
12635,
12636,
12644,
12646,
12653,
12654,
12657,
12659,
12664,
12670,
12671,
12678,
12680,
12692,
12695,
12696,
12697,
12708,
12717,
12723,
12724,
12725,
12727,
12728,
12745,
12747,
12761,
12784,
12785,
12786,
12811,
12816,
12846,
12847,
13503,
13524,
13620,
13621,
13891,
13892,
13900,
13901,
13902,
13908,
14018,
14678,
14679,
15336,
15338,
15339,
15344,
15354,
16822,
16843,
16845,
16847,
};
void ves_icall_System_Array_InternalCreate (int,int,int,int,int);
int ves_icall_System_Array_GetCorElementTypeOfElementTypeInternal (int);
int ves_icall_System_Array_IsValueOfElementTypeInternal (int,int);
int ves_icall_System_Array_CanChangePrimitive (int,int,int);
int ves_icall_System_Array_FastCopy (int,int,int,int,int);
int ves_icall_System_Array_GetLengthInternal_raw (int,int,int);
int ves_icall_System_Array_GetLowerBoundInternal_raw (int,int,int);
void ves_icall_System_Array_GetGenericValue_icall (int,int,int);
void ves_icall_System_Array_GetValueImpl_raw (int,int,int,int);
void ves_icall_System_Array_SetGenericValue_icall (int,int,int);
void ves_icall_System_Array_SetValueImpl_raw (int,int,int,int);
void ves_icall_System_Array_InitializeInternal_raw (int,int);
void ves_icall_System_Array_SetValueRelaxedImpl_raw (int,int,int,int);
void ves_icall_System_Runtime_RuntimeImports_ZeroMemory (int,int);
void ves_icall_System_Runtime_RuntimeImports_Memmove (int,int,int);
void ves_icall_System_Buffer_BulkMoveWithWriteBarrier (int,int,int,int);
int ves_icall_System_Delegate_AllocDelegateLike_internal_raw (int,int);
int ves_icall_System_Delegate_CreateDelegate_internal_raw (int,int,int,int,int);
int ves_icall_System_Delegate_GetVirtualMethod_internal_raw (int,int);
void ves_icall_System_Enum_GetEnumValuesAndNames_raw (int,int,int,int);
void ves_icall_System_Enum_InternalBoxEnum_raw (int,int,int64_t,int);
int ves_icall_System_Enum_InternalGetCorElementType (int);
void ves_icall_System_Enum_InternalGetUnderlyingType_raw (int,int,int);
int ves_icall_System_Environment_get_ProcessorCount ();
int ves_icall_System_Environment_get_TickCount ();
int64_t ves_icall_System_Environment_get_TickCount64 ();
void ves_icall_System_Environment_FailFast_raw (int,int,int,int);
int ves_icall_System_GC_GetCollectionCount (int);
void ves_icall_System_GC_register_ephemeron_array_raw (int,int);
int ves_icall_System_GC_get_ephemeron_tombstone_raw (int);
void ves_icall_System_GC_SuppressFinalize_raw (int,int);
void ves_icall_System_GC_ReRegisterForFinalize_raw (int,int);
void ves_icall_System_GC_GetGCMemoryInfo (int,int,int,int,int,int);
int ves_icall_System_GC_AllocPinnedArray_raw (int,int,int);
int ves_icall_System_Object_MemberwiseClone_raw (int,int);
double ves_icall_System_Math_Acos (double);
double ves_icall_System_Math_Acosh (double);
double ves_icall_System_Math_Asin (double);
double ves_icall_System_Math_Asinh (double);
double ves_icall_System_Math_Atan (double);
double ves_icall_System_Math_Atan2 (double,double);
double ves_icall_System_Math_Atanh (double);
double ves_icall_System_Math_Cbrt (double);
double ves_icall_System_Math_Ceiling (double);
double ves_icall_System_Math_Cos (double);
double ves_icall_System_Math_Cosh (double);
double ves_icall_System_Math_Exp (double);
double ves_icall_System_Math_Floor (double);
double ves_icall_System_Math_Log (double);
double ves_icall_System_Math_Log10 (double);
double ves_icall_System_Math_Pow (double,double);
double ves_icall_System_Math_Sin (double);
double ves_icall_System_Math_Sinh (double);
double ves_icall_System_Math_Sqrt (double);
double ves_icall_System_Math_Tan (double);
double ves_icall_System_Math_Tanh (double);
double ves_icall_System_Math_FusedMultiplyAdd (double,double,double);
double ves_icall_System_Math_Log2 (double);
double ves_icall_System_Math_ModF (double,int);
float ves_icall_System_MathF_Acos (float);
float ves_icall_System_MathF_Acosh (float);
float ves_icall_System_MathF_Asin (float);
float ves_icall_System_MathF_Asinh (float);
float ves_icall_System_MathF_Atan (float);
float ves_icall_System_MathF_Atan2 (float,float);
float ves_icall_System_MathF_Atanh (float);
float ves_icall_System_MathF_Cbrt (float);
float ves_icall_System_MathF_Ceiling (float);
float ves_icall_System_MathF_Cos (float);
float ves_icall_System_MathF_Cosh (float);
float ves_icall_System_MathF_Exp (float);
float ves_icall_System_MathF_Floor (float);
float ves_icall_System_MathF_Log (float);
float ves_icall_System_MathF_Log10 (float);
float ves_icall_System_MathF_Pow (float,float);
float ves_icall_System_MathF_Sin (float);
float ves_icall_System_MathF_Sinh (float);
float ves_icall_System_MathF_Sqrt (float);
float ves_icall_System_MathF_Tan (float);
float ves_icall_System_MathF_Tanh (float);
float ves_icall_System_MathF_FusedMultiplyAdd (float,float,float);
float ves_icall_System_MathF_Log2 (float);
float ves_icall_System_MathF_ModF (float,int);
void ves_icall_RuntimeMethodHandle_ReboxFromNullable_raw (int,int,int);
void ves_icall_RuntimeMethodHandle_ReboxToNullable_raw (int,int,int,int);
int ves_icall_RuntimeType_GetCorrespondingInflatedMethod_raw (int,int,int);
void ves_icall_RuntimeType_make_array_type_raw (int,int,int,int);
void ves_icall_RuntimeType_make_byref_type_raw (int,int,int);
void ves_icall_RuntimeType_make_pointer_type_raw (int,int,int);
void ves_icall_RuntimeType_MakeGenericType_raw (int,int,int,int);
int ves_icall_RuntimeType_GetMethodsByName_native_raw (int,int,int,int,int);
int ves_icall_RuntimeType_GetPropertiesByName_native_raw (int,int,int,int,int);
int ves_icall_RuntimeType_GetConstructors_native_raw (int,int,int);
void ves_icall_RuntimeType_GetInterfaceMapData_raw (int,int,int,int,int);
int ves_icall_System_RuntimeType_CreateInstanceInternal_raw (int,int);
void ves_icall_System_RuntimeType_AllocateValueType_raw (int,int,int,int);
void ves_icall_RuntimeType_GetDeclaringMethod_raw (int,int,int);
void ves_icall_System_RuntimeType_getFullName_raw (int,int,int,int,int);
void ves_icall_RuntimeType_GetGenericArgumentsInternal_raw (int,int,int,int);
int ves_icall_RuntimeType_GetGenericParameterPosition (int);
int ves_icall_RuntimeType_GetEvents_native_raw (int,int,int,int);
int ves_icall_RuntimeType_GetFields_native_raw (int,int,int,int,int);
void ves_icall_RuntimeType_GetInterfaces_raw (int,int,int);
int ves_icall_RuntimeType_GetNestedTypes_native_raw (int,int,int,int,int);
void ves_icall_RuntimeType_GetDeclaringType_raw (int,int,int);
void ves_icall_RuntimeType_GetName_raw (int,int,int);
void ves_icall_RuntimeType_GetNamespace_raw (int,int,int);
int ves_icall_RuntimeType_FunctionPointerReturnAndParameterTypes_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetAttributes (int);
int ves_icall_RuntimeTypeHandle_GetMetadataToken_raw (int,int);
void ves_icall_RuntimeTypeHandle_GetGenericTypeDefinition_impl_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_GetCorElementType (int);
int ves_icall_RuntimeTypeHandle_HasInstantiation (int);
int ves_icall_RuntimeTypeHandle_IsComObject_raw (int,int);
int ves_icall_RuntimeTypeHandle_IsInstanceOfType_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_HasReferences_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetArrayRank_raw (int,int);
void ves_icall_RuntimeTypeHandle_GetAssembly_raw (int,int,int);
void ves_icall_RuntimeTypeHandle_GetElementType_raw (int,int,int);
void ves_icall_RuntimeTypeHandle_GetModule_raw (int,int,int);
void ves_icall_RuntimeTypeHandle_GetBaseType_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_type_is_assignable_from_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_IsGenericTypeDefinition (int);
int ves_icall_RuntimeTypeHandle_GetGenericParameterInfo_raw (int,int);
int ves_icall_RuntimeTypeHandle_is_subclass_of_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_IsByRefLike_raw (int,int);
void ves_icall_System_RuntimeTypeHandle_internal_from_name_raw (int,int,int,int,int,int);
int ves_icall_System_String_FastAllocateString_raw (int,int);
int ves_icall_System_String_InternalIsInterned_raw (int,int);
int ves_icall_System_String_InternalIntern_raw (int,int);
int ves_icall_System_Type_internal_from_handle_raw (int,int);
int ves_icall_System_ValueType_InternalGetHashCode_raw (int,int,int);
int ves_icall_System_ValueType_Equals_raw (int,int,int,int);
int ves_icall_System_Threading_Interlocked_CompareExchange_Int (int,int,int);
void ves_icall_System_Threading_Interlocked_CompareExchange_Object (int,int,int,int);
int ves_icall_System_Threading_Interlocked_Decrement_Int (int);
int64_t ves_icall_System_Threading_Interlocked_Decrement_Long (int);
int ves_icall_System_Threading_Interlocked_Increment_Int (int);
int64_t ves_icall_System_Threading_Interlocked_Increment_Long (int);
int ves_icall_System_Threading_Interlocked_Exchange_Int (int,int);
void ves_icall_System_Threading_Interlocked_Exchange_Object (int,int,int);
int64_t ves_icall_System_Threading_Interlocked_CompareExchange_Long (int,int64_t,int64_t);
int64_t ves_icall_System_Threading_Interlocked_Exchange_Long (int,int64_t);
int64_t ves_icall_System_Threading_Interlocked_Read_Long (int);
int ves_icall_System_Threading_Interlocked_Add_Int (int,int);
int64_t ves_icall_System_Threading_Interlocked_Add_Long (int,int64_t);
void ves_icall_System_Threading_Monitor_Monitor_Enter_raw (int,int);
void mono_monitor_exit_icall_raw (int,int);
void ves_icall_System_Threading_Monitor_Monitor_pulse_raw (int,int);
void ves_icall_System_Threading_Monitor_Monitor_pulse_all_raw (int,int);
int ves_icall_System_Threading_Monitor_Monitor_wait_raw (int,int,int,int);
void ves_icall_System_Threading_Monitor_Monitor_try_enter_with_atomic_var_raw (int,int,int,int,int);
void ves_icall_System_Threading_Thread_StartInternal_raw (int,int,int);
void ves_icall_System_Threading_Thread_InitInternal_raw (int,int);
int ves_icall_System_Threading_Thread_GetCurrentThread ();
void ves_icall_System_Threading_InternalThread_Thread_free_internal_raw (int,int);
int ves_icall_System_Threading_Thread_GetState_raw (int,int);
void ves_icall_System_Threading_Thread_SetState_raw (int,int,int);
void ves_icall_System_Threading_Thread_ClrState_raw (int,int,int);
void ves_icall_System_Threading_Thread_SetName_icall_raw (int,int,int,int);
int ves_icall_System_Threading_Thread_YieldInternal ();
int ves_icall_System_Threading_Thread_Join_internal_raw (int,int,int);
void ves_icall_System_Threading_Thread_SetPriority_raw (int,int,int);
void ves_icall_System_Runtime_Loader_AssemblyLoadContext_PrepareForAssemblyLoadContextRelease_raw (int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_GetLoadContextForAssembly_raw (int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFile_raw (int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalInitializeNativeALC_raw (int,int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFromStream_raw (int,int,int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalGetLoadedAssemblies_raw (int);
int ves_icall_System_GCHandle_InternalAlloc_raw (int,int,int);
void ves_icall_System_GCHandle_InternalFree_raw (int,int);
int ves_icall_System_GCHandle_InternalGet_raw (int,int);
void ves_icall_System_GCHandle_InternalSet_raw (int,int,int);
int ves_icall_System_Runtime_InteropServices_Marshal_GetLastPInvokeError ();
void ves_icall_System_Runtime_InteropServices_Marshal_SetLastPInvokeError (int);
void ves_icall_System_Runtime_InteropServices_Marshal_StructureToPtr_raw (int,int,int,int);
void ves_icall_System_Runtime_InteropServices_Marshal_PtrToStructureInternal_raw (int,int,int,int);
int ves_icall_System_Runtime_InteropServices_Marshal_SizeOfHelper_raw (int,int,int);
int ves_icall_System_Runtime_InteropServices_NativeLibrary_LoadByName_raw (int,int,int,int,int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InternalGetHashCode_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InternalTryGetHashCode_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetObjectValue_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetUninitializedObjectInternal_raw (int,int);
void ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InitializeArray_raw (int,int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetSpanDataFrom_raw (int,int,int,int);
void ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_RunClassConstructor_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_SufficientExecutionStack ();
int ves_icall_System_Reflection_Assembly_GetExecutingAssembly_raw (int,int);
int ves_icall_System_Reflection_Assembly_GetEntryAssembly_raw (int);
int ves_icall_System_Reflection_Assembly_InternalLoad_raw (int,int,int,int);
int ves_icall_System_Reflection_Assembly_InternalGetType_raw (int,int,int,int,int,int);
int ves_icall_System_Reflection_AssemblyName_GetNativeName (int);
int ves_icall_MonoCustomAttrs_GetCustomAttributesInternal_raw (int,int,int,int);
int ves_icall_MonoCustomAttrs_GetCustomAttributesDataInternal_raw (int,int);
int ves_icall_MonoCustomAttrs_IsDefinedInternal_raw (int,int,int);
int ves_icall_System_Reflection_FieldInfo_internal_from_handle_type_raw (int,int,int);
int ves_icall_System_Reflection_FieldInfo_get_marshal_info_raw (int,int);
int ves_icall_System_Reflection_LoaderAllocatorScout_Destroy (int);
void ves_icall_System_Reflection_RuntimeAssembly_GetEntryPoint_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceNames_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeAssembly_GetExportedTypes_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeAssembly_GetInfo_raw (int,int,int,int);
int ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceInternal_raw (int,int,int,int,int);
void ves_icall_System_Reflection_Assembly_GetManifestModuleInternal_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeAssembly_GetModulesInternal_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeCustomAttributeData_ResolveArgumentsInternal_raw (int,int,int,int,int,int,int);
void ves_icall_RuntimeEventInfo_get_event_info_raw (int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_EventInfo_internal_from_handle_type_raw (int,int,int);
int ves_icall_RuntimeFieldInfo_ResolveType_raw (int,int);
int ves_icall_RuntimeFieldInfo_GetParentType_raw (int,int,int);
int ves_icall_RuntimeFieldInfo_GetFieldOffset_raw (int,int);
int ves_icall_RuntimeFieldInfo_GetValueInternal_raw (int,int,int);
void ves_icall_RuntimeFieldInfo_SetValueInternal_raw (int,int,int,int);
int ves_icall_RuntimeFieldInfo_GetRawConstantValue_raw (int,int);
int ves_icall_reflection_get_token_raw (int,int);
void ves_icall_get_method_info_raw (int,int,int);
int ves_icall_get_method_attributes (int);
int ves_icall_System_Reflection_MonoMethodInfo_get_parameter_info_raw (int,int,int);
int ves_icall_System_MonoMethodInfo_get_retval_marshal_raw (int,int);
int ves_icall_System_Reflection_RuntimeMethodInfo_GetMethodFromHandleInternalType_native_raw (int,int,int,int);
int ves_icall_RuntimeMethodInfo_get_name_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_base_method_raw (int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_InternalInvoke_raw (int,int,int,int,int);
void ves_icall_RuntimeMethodInfo_GetPInvoke_raw (int,int,int,int,int);
int ves_icall_RuntimeMethodInfo_MakeGenericMethod_impl_raw (int,int,int);
int ves_icall_RuntimeMethodInfo_GetGenericArguments_raw (int,int);
int ves_icall_RuntimeMethodInfo_GetGenericMethodDefinition_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_IsGenericMethodDefinition_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_IsGenericMethod_raw (int,int);
void ves_icall_InvokeClassConstructor_raw (int,int);
int ves_icall_InternalInvoke_raw (int,int,int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_RuntimeModule_InternalGetTypes_raw (int,int);
void ves_icall_System_Reflection_RuntimeModule_GetGuidInternal_raw (int,int,int);
int ves_icall_System_Reflection_RuntimeModule_ResolveMethodToken_raw (int,int,int,int,int,int);
int ves_icall_RuntimeParameterInfo_GetTypeModifiers_raw (int,int,int,int,int,int);
void ves_icall_RuntimePropertyInfo_get_property_info_raw (int,int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_RuntimePropertyInfo_internal_from_handle_type_raw (int,int,int);
int ves_icall_CustomAttributeBuilder_GetBlob_raw (int,int,int,int,int,int,int,int);
void ves_icall_DynamicMethod_create_dynamic_method_raw (int,int,int,int,int);
void ves_icall_AssemblyBuilder_basic_init_raw (int,int);
void ves_icall_AssemblyBuilder_UpdateNativeCustomAttributes_raw (int,int);
void ves_icall_ModuleBuilder_basic_init_raw (int,int);
void ves_icall_ModuleBuilder_set_wrappers_type_raw (int,int,int);
int ves_icall_ModuleBuilder_getUSIndex_raw (int,int,int);
int ves_icall_ModuleBuilder_getToken_raw (int,int,int,int);
int ves_icall_ModuleBuilder_getMethodToken_raw (int,int,int,int);
void ves_icall_ModuleBuilder_RegisterToken_raw (int,int,int,int);
int ves_icall_TypeBuilder_create_runtime_class_raw (int,int);
int ves_icall_System_IO_Stream_HasOverriddenBeginEndRead_raw (int,int);
int ves_icall_System_IO_Stream_HasOverriddenBeginEndWrite_raw (int,int);
int ves_icall_System_Diagnostics_Debugger_IsAttached_internal ();
int ves_icall_System_Diagnostics_Debugger_IsLogging ();
void ves_icall_System_Diagnostics_Debugger_Log (int,int,int);
int ves_icall_System_Diagnostics_StackFrame_GetFrameInfo (int,int,int,int,int,int,int,int);
void ves_icall_System_Diagnostics_StackTrace_GetTrace (int,int,int,int);
int ves_icall_Mono_RuntimeClassHandle_GetTypeFromClass (int);
void ves_icall_Mono_RuntimeGPtrArrayHandle_GPtrArrayFree (int);
int ves_icall_Mono_SafeStringMarshal_StringToUtf8 (int);
void ves_icall_Mono_SafeStringMarshal_GFree (int);
static void *corlib_icall_funcs [] = {
// token 261,
ves_icall_System_Array_InternalCreate,
// token 273,
ves_icall_System_Array_GetCorElementTypeOfElementTypeInternal,
// token 274,
ves_icall_System_Array_IsValueOfElementTypeInternal,
// token 275,
ves_icall_System_Array_CanChangePrimitive,
// token 276,
ves_icall_System_Array_FastCopy,
// token 277,
ves_icall_System_Array_GetLengthInternal_raw,
// token 278,
ves_icall_System_Array_GetLowerBoundInternal_raw,
// token 279,
ves_icall_System_Array_GetGenericValue_icall,
// token 280,
ves_icall_System_Array_GetValueImpl_raw,
// token 281,
ves_icall_System_Array_SetGenericValue_icall,
// token 284,
ves_icall_System_Array_SetValueImpl_raw,
// token 285,
ves_icall_System_Array_InitializeInternal_raw,
// token 286,
ves_icall_System_Array_SetValueRelaxedImpl_raw,
// token 462,
ves_icall_System_Runtime_RuntimeImports_ZeroMemory,
// token 463,
ves_icall_System_Runtime_RuntimeImports_Memmove,
// token 464,
ves_icall_System_Buffer_BulkMoveWithWriteBarrier,
// token 494,
ves_icall_System_Delegate_AllocDelegateLike_internal_raw,
// token 495,
ves_icall_System_Delegate_CreateDelegate_internal_raw,
// token 496,
ves_icall_System_Delegate_GetVirtualMethod_internal_raw,
// token 516,
ves_icall_System_Enum_GetEnumValuesAndNames_raw,
// token 517,
ves_icall_System_Enum_InternalBoxEnum_raw,
// token 518,
ves_icall_System_Enum_InternalGetCorElementType,
// token 519,
ves_icall_System_Enum_InternalGetUnderlyingType_raw,
// token 636,
ves_icall_System_Environment_get_ProcessorCount,
// token 637,
ves_icall_System_Environment_get_TickCount,
// token 638,
ves_icall_System_Environment_get_TickCount64,
// token 641,
ves_icall_System_Environment_FailFast_raw,
// token 691,
ves_icall_System_GC_GetCollectionCount,
// token 692,
ves_icall_System_GC_register_ephemeron_array_raw,
// token 693,
ves_icall_System_GC_get_ephemeron_tombstone_raw,
// token 696,
ves_icall_System_GC_SuppressFinalize_raw,
// token 698,
ves_icall_System_GC_ReRegisterForFinalize_raw,
// token 700,
ves_icall_System_GC_GetGCMemoryInfo,
// token 702,
ves_icall_System_GC_AllocPinnedArray_raw,
// token 707,
ves_icall_System_Object_MemberwiseClone_raw,
// token 715,
ves_icall_System_Math_Acos,
// token 716,
ves_icall_System_Math_Acosh,
// token 717,
ves_icall_System_Math_Asin,
// token 718,
ves_icall_System_Math_Asinh,
// token 719,
ves_icall_System_Math_Atan,
// token 720,
ves_icall_System_Math_Atan2,
// token 721,
ves_icall_System_Math_Atanh,
// token 722,
ves_icall_System_Math_Cbrt,
// token 723,
ves_icall_System_Math_Ceiling,
// token 724,
ves_icall_System_Math_Cos,
// token 725,
ves_icall_System_Math_Cosh,
// token 726,
ves_icall_System_Math_Exp,
// token 727,
ves_icall_System_Math_Floor,
// token 728,
ves_icall_System_Math_Log,
// token 729,
ves_icall_System_Math_Log10,
// token 730,
ves_icall_System_Math_Pow,
// token 731,
ves_icall_System_Math_Sin,
// token 733,
ves_icall_System_Math_Sinh,
// token 734,
ves_icall_System_Math_Sqrt,
// token 735,
ves_icall_System_Math_Tan,
// token 736,
ves_icall_System_Math_Tanh,
// token 737,
ves_icall_System_Math_FusedMultiplyAdd,
// token 738,
ves_icall_System_Math_Log2,
// token 739,
ves_icall_System_Math_ModF,
// token 836,
ves_icall_System_MathF_Acos,
// token 837,
ves_icall_System_MathF_Acosh,
// token 838,
ves_icall_System_MathF_Asin,
// token 839,
ves_icall_System_MathF_Asinh,
// token 840,
ves_icall_System_MathF_Atan,
// token 841,
ves_icall_System_MathF_Atan2,
// token 842,
ves_icall_System_MathF_Atanh,
// token 843,
ves_icall_System_MathF_Cbrt,
// token 844,
ves_icall_System_MathF_Ceiling,
// token 845,
ves_icall_System_MathF_Cos,
// token 846,
ves_icall_System_MathF_Cosh,
// token 847,
ves_icall_System_MathF_Exp,
// token 848,
ves_icall_System_MathF_Floor,
// token 849,
ves_icall_System_MathF_Log,
// token 850,
ves_icall_System_MathF_Log10,
// token 851,
ves_icall_System_MathF_Pow,
// token 852,
ves_icall_System_MathF_Sin,
// token 854,
ves_icall_System_MathF_Sinh,
// token 855,
ves_icall_System_MathF_Sqrt,
// token 856,
ves_icall_System_MathF_Tan,
// token 857,
ves_icall_System_MathF_Tanh,
// token 858,
ves_icall_System_MathF_FusedMultiplyAdd,
// token 859,
ves_icall_System_MathF_Log2,
// token 860,
ves_icall_System_MathF_ModF,
// token 927,
ves_icall_RuntimeMethodHandle_ReboxFromNullable_raw,
// token 928,
ves_icall_RuntimeMethodHandle_ReboxToNullable_raw,
// token 998,
ves_icall_RuntimeType_GetCorrespondingInflatedMethod_raw,
// token 1005,
ves_icall_RuntimeType_make_array_type_raw,
// token 1008,
ves_icall_RuntimeType_make_byref_type_raw,
// token 1010,
ves_icall_RuntimeType_make_pointer_type_raw,
// token 1015,
ves_icall_RuntimeType_MakeGenericType_raw,
// token 1016,
ves_icall_RuntimeType_GetMethodsByName_native_raw,
// token 1018,
ves_icall_RuntimeType_GetPropertiesByName_native_raw,
// token 1019,
ves_icall_RuntimeType_GetConstructors_native_raw,
// token 1023,
ves_icall_RuntimeType_GetInterfaceMapData_raw,
// token 1025,
ves_icall_System_RuntimeType_CreateInstanceInternal_raw,
// token 1026,
ves_icall_System_RuntimeType_AllocateValueType_raw,
// token 1028,
ves_icall_RuntimeType_GetDeclaringMethod_raw,
// token 1030,
ves_icall_System_RuntimeType_getFullName_raw,
// token 1031,
ves_icall_RuntimeType_GetGenericArgumentsInternal_raw,
// token 1034,
ves_icall_RuntimeType_GetGenericParameterPosition,
// token 1035,
ves_icall_RuntimeType_GetEvents_native_raw,
// token 1036,
ves_icall_RuntimeType_GetFields_native_raw,
// token 1039,
ves_icall_RuntimeType_GetInterfaces_raw,
// token 1041,
ves_icall_RuntimeType_GetNestedTypes_native_raw,
// token 1044,
ves_icall_RuntimeType_GetDeclaringType_raw,
// token 1046,
ves_icall_RuntimeType_GetName_raw,
// token 1048,
ves_icall_RuntimeType_GetNamespace_raw,
// token 1057,
ves_icall_RuntimeType_FunctionPointerReturnAndParameterTypes_raw,
// token 1127,
ves_icall_RuntimeTypeHandle_GetAttributes,
// token 1129,
ves_icall_RuntimeTypeHandle_GetMetadataToken_raw,
// token 1131,
ves_icall_RuntimeTypeHandle_GetGenericTypeDefinition_impl_raw,
// token 1141,
ves_icall_RuntimeTypeHandle_GetCorElementType,
// token 1142,
ves_icall_RuntimeTypeHandle_HasInstantiation,
// token 1143,
ves_icall_RuntimeTypeHandle_IsComObject_raw,
// token 1144,
ves_icall_RuntimeTypeHandle_IsInstanceOfType_raw,
// token 1146,
ves_icall_RuntimeTypeHandle_HasReferences_raw,
// token 1153,
ves_icall_RuntimeTypeHandle_GetArrayRank_raw,
// token 1154,
ves_icall_RuntimeTypeHandle_GetAssembly_raw,
// token 1155,
ves_icall_RuntimeTypeHandle_GetElementType_raw,
// token 1156,
ves_icall_RuntimeTypeHandle_GetModule_raw,
// token 1157,
ves_icall_RuntimeTypeHandle_GetBaseType_raw,
// token 1165,
ves_icall_RuntimeTypeHandle_type_is_assignable_from_raw,
// token 1166,
ves_icall_RuntimeTypeHandle_IsGenericTypeDefinition,
// token 1167,
ves_icall_RuntimeTypeHandle_GetGenericParameterInfo_raw,
// token 1171,
ves_icall_RuntimeTypeHandle_is_subclass_of_raw,
// token 1172,
ves_icall_RuntimeTypeHandle_IsByRefLike_raw,
// token 1174,
ves_icall_System_RuntimeTypeHandle_internal_from_name_raw,
// token 1178,
ves_icall_System_String_FastAllocateString_raw,
// token 1179,
ves_icall_System_String_InternalIsInterned_raw,
// token 1180,
ves_icall_System_String_InternalIntern_raw,
// token 1464,
ves_icall_System_Type_internal_from_handle_raw,
// token 1666,
ves_icall_System_ValueType_InternalGetHashCode_raw,
// token 1667,
ves_icall_System_ValueType_Equals_raw,
// token 10046,
ves_icall_System_Threading_Interlocked_CompareExchange_Int,
// token 10047,
ves_icall_System_Threading_Interlocked_CompareExchange_Object,
// token 10049,
ves_icall_System_Threading_Interlocked_Decrement_Int,
// token 10050,
ves_icall_System_Threading_Interlocked_Decrement_Long,
// token 10051,
ves_icall_System_Threading_Interlocked_Increment_Int,
// token 10052,
ves_icall_System_Threading_Interlocked_Increment_Long,
// token 10053,
ves_icall_System_Threading_Interlocked_Exchange_Int,
// token 10054,
ves_icall_System_Threading_Interlocked_Exchange_Object,
// token 10056,
ves_icall_System_Threading_Interlocked_CompareExchange_Long,
// token 10058,
ves_icall_System_Threading_Interlocked_Exchange_Long,
// token 10060,
ves_icall_System_Threading_Interlocked_Read_Long,
// token 10061,
ves_icall_System_Threading_Interlocked_Add_Int,
// token 10062,
ves_icall_System_Threading_Interlocked_Add_Long,
// token 10073,
ves_icall_System_Threading_Monitor_Monitor_Enter_raw,
// token 10075,
mono_monitor_exit_icall_raw,
// token 10080,
ves_icall_System_Threading_Monitor_Monitor_pulse_raw,
// token 10082,
ves_icall_System_Threading_Monitor_Monitor_pulse_all_raw,
// token 10084,
ves_icall_System_Threading_Monitor_Monitor_wait_raw,
// token 10086,
ves_icall_System_Threading_Monitor_Monitor_try_enter_with_atomic_var_raw,
// token 10138,
ves_icall_System_Threading_Thread_StartInternal_raw,
// token 10145,
ves_icall_System_Threading_Thread_InitInternal_raw,
// token 10146,
ves_icall_System_Threading_Thread_GetCurrentThread,
// token 10148,
ves_icall_System_Threading_InternalThread_Thread_free_internal_raw,
// token 10149,
ves_icall_System_Threading_Thread_GetState_raw,
// token 10150,
ves_icall_System_Threading_Thread_SetState_raw,
// token 10151,
ves_icall_System_Threading_Thread_ClrState_raw,
// token 10152,
ves_icall_System_Threading_Thread_SetName_icall_raw,
// token 10154,
ves_icall_System_Threading_Thread_YieldInternal,
// token 10156,
ves_icall_System_Threading_Thread_Join_internal_raw,
// token 10157,
ves_icall_System_Threading_Thread_SetPriority_raw,
// token 11386,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_PrepareForAssemblyLoadContextRelease_raw,
// token 11390,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_GetLoadContextForAssembly_raw,
// token 11392,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFile_raw,
// token 11393,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalInitializeNativeALC_raw,
// token 11394,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFromStream_raw,
// token 11395,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalGetLoadedAssemblies_raw,
// token 11662,
ves_icall_System_GCHandle_InternalAlloc_raw,
// token 11663,
ves_icall_System_GCHandle_InternalFree_raw,
// token 11664,
ves_icall_System_GCHandle_InternalGet_raw,
// token 11665,
ves_icall_System_GCHandle_InternalSet_raw,
// token 11685,
ves_icall_System_Runtime_InteropServices_Marshal_GetLastPInvokeError,
// token 11686,
ves_icall_System_Runtime_InteropServices_Marshal_SetLastPInvokeError,
// token 11687,
ves_icall_System_Runtime_InteropServices_Marshal_StructureToPtr_raw,
// token 11689,
ves_icall_System_Runtime_InteropServices_Marshal_PtrToStructureInternal_raw,
// token 11691,
ves_icall_System_Runtime_InteropServices_Marshal_SizeOfHelper_raw,
// token 11754,
ves_icall_System_Runtime_InteropServices_NativeLibrary_LoadByName_raw,
// token 11839,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InternalGetHashCode_raw,
// token 11841,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InternalTryGetHashCode_raw,
// token 11843,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetObjectValue_raw,
// token 11853,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetUninitializedObjectInternal_raw,
// token 11854,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InitializeArray_raw,
// token 11855,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetSpanDataFrom_raw,
// token 11856,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_RunClassConstructor_raw,
// token 11857,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_SufficientExecutionStack,
// token 12343,
ves_icall_System_Reflection_Assembly_GetExecutingAssembly_raw,
// token 12344,
ves_icall_System_Reflection_Assembly_GetEntryAssembly_raw,
// token 12349,
ves_icall_System_Reflection_Assembly_InternalLoad_raw,
// token 12350,
ves_icall_System_Reflection_Assembly_InternalGetType_raw,
// token 12389,
ves_icall_System_Reflection_AssemblyName_GetNativeName,
// token 12435,
ves_icall_MonoCustomAttrs_GetCustomAttributesInternal_raw,
// token 12442,
ves_icall_MonoCustomAttrs_GetCustomAttributesDataInternal_raw,
// token 12449,
ves_icall_MonoCustomAttrs_IsDefinedInternal_raw,
// token 12460,
ves_icall_System_Reflection_FieldInfo_internal_from_handle_type_raw,
// token 12464,
ves_icall_System_Reflection_FieldInfo_get_marshal_info_raw,
// token 12490,
ves_icall_System_Reflection_LoaderAllocatorScout_Destroy,
// token 12570,
ves_icall_System_Reflection_RuntimeAssembly_GetEntryPoint_raw,
// token 12576,
ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceNames_raw,
// token 12578,
ves_icall_System_Reflection_RuntimeAssembly_GetExportedTypes_raw,
// token 12589,
ves_icall_System_Reflection_RuntimeAssembly_GetInfo_raw,
// token 12591,
ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceInternal_raw,
// token 12592,
ves_icall_System_Reflection_Assembly_GetManifestModuleInternal_raw,
// token 12593,
ves_icall_System_Reflection_RuntimeAssembly_GetModulesInternal_raw,
// token 12600,
ves_icall_System_Reflection_RuntimeCustomAttributeData_ResolveArgumentsInternal_raw,
// token 12615,
ves_icall_RuntimeEventInfo_get_event_info_raw,
// token 12635,
ves_icall_reflection_get_token_raw,
// token 12636,
ves_icall_System_Reflection_EventInfo_internal_from_handle_type_raw,
// token 12644,
ves_icall_RuntimeFieldInfo_ResolveType_raw,
// token 12646,
ves_icall_RuntimeFieldInfo_GetParentType_raw,
// token 12653,
ves_icall_RuntimeFieldInfo_GetFieldOffset_raw,
// token 12654,
ves_icall_RuntimeFieldInfo_GetValueInternal_raw,
// token 12657,
ves_icall_RuntimeFieldInfo_SetValueInternal_raw,
// token 12659,
ves_icall_RuntimeFieldInfo_GetRawConstantValue_raw,
// token 12664,
ves_icall_reflection_get_token_raw,
// token 12670,
ves_icall_get_method_info_raw,
// token 12671,
ves_icall_get_method_attributes,
// token 12678,
ves_icall_System_Reflection_MonoMethodInfo_get_parameter_info_raw,
// token 12680,
ves_icall_System_MonoMethodInfo_get_retval_marshal_raw,
// token 12692,
ves_icall_System_Reflection_RuntimeMethodInfo_GetMethodFromHandleInternalType_native_raw,
// token 12695,
ves_icall_RuntimeMethodInfo_get_name_raw,
// token 12696,
ves_icall_RuntimeMethodInfo_get_base_method_raw,
// token 12697,
ves_icall_reflection_get_token_raw,
// token 12708,
ves_icall_InternalInvoke_raw,
// token 12717,
ves_icall_RuntimeMethodInfo_GetPInvoke_raw,
// token 12723,
ves_icall_RuntimeMethodInfo_MakeGenericMethod_impl_raw,
// token 12724,
ves_icall_RuntimeMethodInfo_GetGenericArguments_raw,
// token 12725,
ves_icall_RuntimeMethodInfo_GetGenericMethodDefinition_raw,
// token 12727,
ves_icall_RuntimeMethodInfo_get_IsGenericMethodDefinition_raw,
// token 12728,
ves_icall_RuntimeMethodInfo_get_IsGenericMethod_raw,
// token 12745,
ves_icall_InvokeClassConstructor_raw,
// token 12747,
ves_icall_InternalInvoke_raw,
// token 12761,
ves_icall_reflection_get_token_raw,
// token 12784,
ves_icall_System_Reflection_RuntimeModule_InternalGetTypes_raw,
// token 12785,
ves_icall_System_Reflection_RuntimeModule_GetGuidInternal_raw,
// token 12786,
ves_icall_System_Reflection_RuntimeModule_ResolveMethodToken_raw,
// token 12811,
ves_icall_RuntimeParameterInfo_GetTypeModifiers_raw,
// token 12816,
ves_icall_RuntimePropertyInfo_get_property_info_raw,
// token 12846,
ves_icall_reflection_get_token_raw,
// token 12847,
ves_icall_System_Reflection_RuntimePropertyInfo_internal_from_handle_type_raw,
// token 13503,
ves_icall_CustomAttributeBuilder_GetBlob_raw,
// token 13524,
ves_icall_DynamicMethod_create_dynamic_method_raw,
// token 13620,
ves_icall_AssemblyBuilder_basic_init_raw,
// token 13621,
ves_icall_AssemblyBuilder_UpdateNativeCustomAttributes_raw,
// token 13891,
ves_icall_ModuleBuilder_basic_init_raw,
// token 13892,
ves_icall_ModuleBuilder_set_wrappers_type_raw,
// token 13900,
ves_icall_ModuleBuilder_getUSIndex_raw,
// token 13901,
ves_icall_ModuleBuilder_getToken_raw,
// token 13902,
ves_icall_ModuleBuilder_getMethodToken_raw,
// token 13908,
ves_icall_ModuleBuilder_RegisterToken_raw,
// token 14018,
ves_icall_TypeBuilder_create_runtime_class_raw,
// token 14678,
ves_icall_System_IO_Stream_HasOverriddenBeginEndRead_raw,
// token 14679,
ves_icall_System_IO_Stream_HasOverriddenBeginEndWrite_raw,
// token 15336,
ves_icall_System_Diagnostics_Debugger_IsAttached_internal,
// token 15338,
ves_icall_System_Diagnostics_Debugger_IsLogging,
// token 15339,
ves_icall_System_Diagnostics_Debugger_Log,
// token 15344,
ves_icall_System_Diagnostics_StackFrame_GetFrameInfo,
// token 15354,
ves_icall_System_Diagnostics_StackTrace_GetTrace,
// token 16822,
ves_icall_Mono_RuntimeClassHandle_GetTypeFromClass,
// token 16843,
ves_icall_Mono_RuntimeGPtrArrayHandle_GPtrArrayFree,
// token 16845,
ves_icall_Mono_SafeStringMarshal_StringToUtf8,
// token 16847,
ves_icall_Mono_SafeStringMarshal_GFree,
};
static uint8_t corlib_icall_flags [] = {
0,
0,
0,
0,
0,
4,
4,
0,
4,
0,
4,
4,
4,
0,
0,
0,
4,
4,
4,
4,
4,
0,
4,
0,
0,
0,
4,
0,
4,
4,
4,
4,
0,
4,
4,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
0,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
0,
0,
0,
0,
0,
0,
0,
0,
};
