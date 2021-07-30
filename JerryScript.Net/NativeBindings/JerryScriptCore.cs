using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace JerryScript.Net.NativeBindings
{
    public class JerryScriptCore
    {
        private const string LibraryName = "jerry-core";

        public const uint JERRY_API_MAJOR_VERSION = 2;
        public const uint JERRY_API_MINOR_VERSION = 4;
        public const uint JERRY_API_PATCH_VERSION = 0;

        [Flags]
        public enum jerry_init_flag_t
        {
            JERRY_INIT_EMPTY = 0,
            JERRY_INIT_SHOW_OPCODES = 1 << 0,
            JERRY_INIT_SHOW_REGEXP_OPCODES = 1 << 1,
            JERRY_INIT_MEM_STATS = 1 << 2
        }

        public enum jerry_error_t
        {
            JERRY_ERROR_NONE,
            JERRY_ERROR_COMMON,
            JERRY_ERROR_EVAL,
            JERRY_ERROR_RANGE,
            JERRY_ERROR_REFERENCE,
            JERRY_ERROR_SYNTAX,
            JERRY_ERROR_TYPE,
            JERRY_ERROR_URI
        }

        public enum jerry_feature_t
        {
            JERRY_FEATURE_CPOINTER_32_BIT,
            JERRY_FEATURE_ERROR_MESSAGES,
            JERRY_FEATURE_JS_PARSER,
            JERRY_FEATURE_MEM_STATS,
            JERRY_FEATURE_PARSER_DUMP,
            JERRY_FEATURE_REGEXP_DUMP,
            JERRY_FEATURE_SNAPSHOT_SAVE,
            JERRY_FEATURE_SNAPSHOT_EXEC,
            JERRY_FEATURE_DEBUGGER,
            JERRY_FEATURE_VM_EXEC_STOP,
            JERRY_FEATURE_JSON,
            JERRY_FEATURE_PROMISE,
            JERRY_FEATURE_TYPEDARRAY,
            JERRY_FEATURE_DATE,
            JERRY_FEATURE_REGEXP,
            JERRY_FEATURE_LINE_INFO,
            JERRY_FEATURE_LOGGING,
            JERRY_FEATURE_SYMBOL,
            JERRY_FEATURE_DATAVIEW,
            JERRY_FEATURE_PROXY,
            JERRY_FEATURE_MAP,
            JERRY_FEATURE_SET,
            JERRY_FEATURE_WEAKMAP,
            JERRY_FEATURE_WEAKSET,
            JERRY_FEATURE_BIGINT,
            JERRY_FEATURE_REALM
        }

        [Flags]
        public enum jerry_parse_opts_t
        {
            JERRY_PARSE_NO_OPTS = 0,
            JERRY_PARSE_STRICT_MODE = 1 << 0,
            JERRY_PARSE_MODULE = 1 << 1
        }

        public enum jerry_gc_mode_t
        {
            JERRY_GC_PRESSURE_LOW,
            JERRY_GC_PRESSURE_HIGH
        }

        [Flags]
        public enum jerry_regexp_flags_t : ushort
        {
            JERRY_REGEXP_FLAG_GLOBAL = 1 << 1,
            JERRY_REGEXP_FLAG_IGNORE_CASE = 1 << 2,
            JERRY_REGEXP_FLAG_MULTILINE = 1 << 3,
            JERRY_REGEXP_FLAG_STICKY = 1 << 4,
            JERRY_REGEXP_FLAG_UNICODE = 1 << 5,
            JERRY_REGEXP_FLAG_DOTALL = 1 << 6
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct jerry_property_descriptor_t
        {
            public bool is_value_defined;
            public bool is_get_defined;
            public bool is_set_defined;
            public bool is_writable_defined;
            public bool is_writable;
            public bool is_enumerable_defined;
            public bool is_enumerable;
            public bool is_configurable_defined;
            public bool is_configurable;
            public uint value;
            public uint getter;
            public uint setter;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct jerry_heap_stats_t
        {
            public uint version;
            public uint size;
            public uint allocated_bytes;
            public uint peak_allocated_bytes;
            public unsafe fixed uint reserved[4];
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate uint jerry_external_handler_t(
            uint function_obj,
            uint this_val,
            uint* args_p,
            uint args_count
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate void jerry_object_native_free_callback_t(void* native_p);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate void jerry_error_object_created_callback_t(uint error_object, void* user_p);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate uint jerry_vm_exec_stop_callback_t(void* user_p);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate bool jerry_object_property_foreach_t(
            uint property_name,
            uint property_value,
            void* user_data_p
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate bool jerry_objects_foreach_by_native_info_t(
            uint obj,
            void* object_data_p,
            void* user_data_p
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate void jerry_context_data_manager_cb_t(void* data);

        [StructLayout(LayoutKind.Sequential)]
        public struct jerry_context_data_manager_t
        {
            public jerry_context_data_manager_cb_t init_cb;
            public jerry_context_data_manager_cb_t deinit_cb;
            public jerry_context_data_manager_cb_t finalize_cb;
            public uint bytes_needed;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate void* jerry_context_alloc_t(uint size, void* cb_data_p);

        [StructLayout(LayoutKind.Sequential)]
        public struct jerry_object_native_info_t
        {
            jerry_object_native_free_callback_t free_cb;
        }

        public enum jerry_binary_operation_t
        {
            JERRY_BIN_OP_EQUAL,
            JERRY_BIN_OP_STRICT_EQUAL,
            JERRY_BIN_OP_LESS,
            JERRY_BIN_OP_LESS_EQUAL,
            JERRY_BIN_OP_GREATER,
            JERRY_BIN_OP_GREATER_EQUAL,
            JERRY_BIN_OP_INSTANCEOF,
            JERRY_BIN_OP_ADD,
            JERRY_BIN_OP_SUB,
            JERRY_BIN_OP_MUL,
            JERRY_BIN_OP_DIV,
            JERRY_BIN_OP_REM
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void jerry_init(jerry_init_flag_t flags);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void jerry_cleanup();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = nameof(jerry_register_magic_strings))]
        private static extern void jerry_register_magic_strings_INTERNAL(
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)]
            string[] ex_str_items_p,
            uint count,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U4)]
            uint[] str_lengths_p
        );

        public static void jerry_register_magic_strings(string[] ex_str_items)
        {
            jerry_register_magic_strings_INTERNAL(
                ex_str_items,
                (uint)ex_str_items.Length,
                ex_str_items.Select(x => (uint)Encoding.UTF8.GetByteCount(x)).ToArray()
            );
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void jerry_gc(jerry_gc_mode_t mode);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr jerry_get_context_data(ref jerry_context_data_manager_t manager_p);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool jerry_get_memory_stats(out jerry_heap_stats_t out_stats_p);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(jerry_run_simple))]
        private static extern bool jerry_run_simple_INTERNAL(
            [MarshalAs(UnmanagedType.LPUTF8Str)] string script_source_p,
            uint script_source_size,
            jerry_init_flag_t flags
        );

        public static bool jerry_run_simple(string script_source, jerry_init_flag_t flags)
        {
            return jerry_run_simple_INTERNAL(
                script_source,
                (uint)Encoding.UTF8.GetByteCount(script_source),
                flags
            );
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(jerry_parse))]
        private static extern uint jerry_parse_INTERNAL(
            [MarshalAs(UnmanagedType.LPUTF8Str)] string resource_name_p,
            uint resource_name_length,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string source_p,
            uint source_size,
            uint parse_opts
        );

        public static uint jerry_parse(string resource_name, string source, uint parse_opts)
        {
            return jerry_parse_INTERNAL(
                resource_name,
                (uint)Encoding.UTF8.GetByteCount(resource_name),
                source,
                (uint)Encoding.UTF8.GetByteCount(source),
                parse_opts
            );
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(jerry_parse_function))]
        private static extern uint jerry_parse_function_INTERNAL(
            [MarshalAs(UnmanagedType.LPUTF8Str)] string resource_name_p,
            uint resource_name_length,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string arg_list_p,
            uint arg_list_size,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string source_p,
            uint source_size,
            uint parse_opts
        );

        public static uint jerry_parse_function(
            string resource_name,
            string[] arg_list,
            string source,
            uint parse_opts
        )
        {
            var argString = string.Join(", ", arg_list);

            return jerry_parse_function_INTERNAL(
                resource_name,
                (uint)Encoding.UTF8.GetByteCount(resource_name),
                argString,
                (uint)Encoding.UTF8.GetByteCount(argString),
                source,
                (uint)Encoding.UTF8.GetByteCount(source),
                parse_opts
            );
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_run(uint func_val);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "jerry_eval")]
        private static extern uint jerry_eval_INTERNAL(
            [MarshalAs(UnmanagedType.LPUTF8Str)] string source_p,
            uint source_size,
            uint parse_opts
        );

        public static uint jerry_eval(string source, uint parse_opts)
        {
            return jerry_eval_INTERNAL(
                source,
                (uint)Encoding.UTF8.GetByteCount(source),
                parse_opts
            );
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_run_all_enqueued_jobs();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_get_global_object();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool jerry_value_is_abort(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool jerry_value_is_array(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool jerry_value_is_boolean(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool jerry_value_is_constructor(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool jerry_value_is_error(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool jerry_value_is_function(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool jerry_value_is_async_function(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool jerry_value_is_number(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool jerry_value_is_null(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool jerry_value_is_object(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool jerry_value_is_promise(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool jerry_value_is_proxy(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool jerry_value_is_string(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool jerry_value_is_symbol(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool jerry_value_is_bigint(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool jerry_value_is_undefined(uint value);

        public enum jerry_type_t
        {
            JERRY_TYPE_NONE,
            JERRY_TYPE_UNDEFINED,
            JERRY_TYPE_NULL,
            JERRY_TYPE_BOOLEAN,
            JERRY_TYPE_NUMBER,
            JERRY_TYPE_STRING,
            JERRY_TYPE_OBJECT,
            JERRY_TYPE_FUNCTION,
            JERRY_TYPE_ERROR,
            JERRY_TYPE_SYMBOL,
            JERRY_TYPE_BIGINT
        }

        public enum jerry_object_type_t
        {
            JERRY_OBJECT_TYPE_NONE,
            JERRY_OBJECT_TYPE_GENERIC,
            JERRY_OBJECT_TYPE_ARRAY,
            JERRY_OBJECT_TYPE_PROXY,
            JERRY_OBJECT_TYPE_FUNCTION,
            JERRY_OBJECT_TYPE_TYPEDARRAY,
            JERRY_OBJECT_TYPE_ITERATOR,
            JERRY_OBJECT_TYPE_CONTAINER,
            JERRY_OBJECT_TYPE_ARGUMENTS,
            JERRY_OBJECT_TYPE_BOOLEAN,
            JERRY_OBJECT_TYPE_DATE,
            JERRY_OBJECT_TYPE_NUMBER,
            JERRY_OBJECT_TYPE_REGEXP,
            JERRY_OBJECT_TYPE_STRING,
            JERRY_OBJECT_TYPE_SYMBOL,
            JERRY_OBJECT_TYPE_GENERATOR,
            JERRY_OBJECT_TYPE_BIGINT
        }

        public enum jerry_function_type_t
        {
            JERRY_FUNCTION_TYPE_NONE,
            JERRY_FUNCTION_TYPE_GENERIC,
            JERRY_FUNCTION_TYPE_ACCESSOR,
            JERRY_FUNCTION_TYPE_BOUND,
            JERRY_FUNCTION_TYPE_ARROW,
            JERRY_FUNCTION_TYPE_GENERATOR
        }

        public enum jerry_iterator_type_t
        {
            JERRY_ITERATOR_TYPE_NONE,
            JERRY_ITERATOR_TYPE_ARRAY,
            JERRY_ITERATOR_TYPE_STRING,
            JERRY_ITERATOR_TYPE_MAP,
            JERRY_ITERATOR_TYPE_SET
        }

        [Flags]
        public enum jerry_property_filter_t
        {
            JERRY_PROPERTY_FILTER_ALL = 0,
            JERRY_PROPERTY_FILTER_TRAVERSE_PROTOTYPE_CHAIN = (1 << 0),
            JERRY_PROPERTY_FILTER_EXCLUDE_NON_CONFIGURABLE = (1 << 1),
            JERRY_PROPERTY_FILTER_EXCLUDE_NON_ENUMERABLE = (1 << 2),
            JERRY_PROPERTY_FILTER_EXCLUDE_NON_WRITABLE = (1 << 3),
            JERRY_PROPERTY_FILTER_EXCLUDE_STRINGS = (1 << 4),
            JERRY_PROPERTY_FILTER_EXCLUDE_SYMBOLS = (1 << 5),
            JERRY_PROPERTY_FILTER_EXCLUDE_INTEGER_INDICES = (1 << 6),
            JERRY_PROPERTY_FILTER_INTEGER_INDICES_AS_NUMBER = (1 << 7)
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern jerry_type_t jerry_value_get_type(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern jerry_object_type_t jerry_object_get_type(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern jerry_function_type_t jerry_function_get_type(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern jerry_iterator_type_t jerry_iterator_get_type(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool jerry_is_feature_enabled(jerry_feature_t feature);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_binary_operation(jerry_binary_operation_t op, uint lhs, uint rhs);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_create_abort_from_value(uint value, bool release);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_create_error_from_value(uint value, bool release);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_get_value_from_error(uint value, bool release);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void jerry_set_error_object_created_callback(
            jerry_error_object_created_callback_t callback);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern jerry_error_t jerry_get_error_type(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool jerry_get_boolean_value(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double jerry_get_number_value(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_get_string_size(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_get_utf8_string_size(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_get_string_length(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_get_utf8_string_length(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = nameof(jerry_string_to_char_buffer))]
        private static extern uint jerry_string_to_char_buffer_INTERNAL(
            uint value,
            [In] byte[] buffer_p,
            uint buffer_size
        );

        public static uint jerry_string_to_char_buffer(uint value, out string buffer)
        {
            var bufsize = jerry_get_string_length(value);
            var buf = new byte[bufsize];
            var retval = jerry_string_to_char_buffer_INTERNAL(
                value,
                buf,
                (uint)buf.Length
            );

            buffer = Encoding.ASCII.GetString(buf);
            return retval;
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = nameof(jerry_string_to_utf8_char_buffer))]
        private static extern uint jerry_string_to_utf8_char_buffer_INTERNAL(
            uint value,
            [In] byte[] buffer_p,
            uint buffer_size
        );

        public static uint jerry_string_to_utf8_char_buffer(uint value, out string buffer)
        {
            var bufsize = jerry_get_utf8_string_length(value);
            var buf = new byte[bufsize];
            var retval = jerry_string_to_utf8_char_buffer_INTERNAL(
                value,
                buf,
                (uint)buf.Length
            );

            buffer = Encoding.UTF8.GetString(buf);
            return retval;
        }

        /*
         * substring_to_X functions intentionally omitted
         * as per docs' recommendations, just use C# functionality
         * if you actually need to extract a substring :rolls_eyes:
         */

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_get_array_length(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool jerry_value_to_boolean(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_value_to_number(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_value_to_object(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_value_to_primitive(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_value_to_string(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_value_to_bigint(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double jerry_value_as_integer(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int jerry_value_as_int32(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_value_as_uint32(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_acquire_value(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void jerry_release_value(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_create_array(uint size);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_create_boolean(bool value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint jerry_create_error_sz(
            jerry_error_t error_type,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string message,
            uint message_size
        );

        public static uint jerry_create_error(jerry_error_t error_type, string message)
        {
            return jerry_create_error_sz(
                error_type,
                message,
                (uint)Encoding.UTF8.GetByteCount(message)
            );
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_create_external_function(jerry_external_handler_t handler_p);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_create_number(double value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_create_number_infinity(bool sign);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_create_number_nan();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_create_null();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_create_object();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_create_promise();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_create_proxy(uint target, uint handler);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint jerry_create_regexp_sz(
            [MarshalAs(UnmanagedType.LPUTF8Str)] string pattern,
            uint pattern_size,
            jerry_regexp_flags_t flags
        );

        public static uint jerry_create_regexp(string pattern, jerry_regexp_flags_t flags)
        {
            return jerry_create_regexp_sz(
                pattern,
                (uint)Encoding.UTF8.GetByteCount(pattern),
                flags
            );
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint jerry_create_string_sz_from_utf8(
            [MarshalAs(UnmanagedType.LPUTF8Str)] string str_p,
            uint str_size
        );

        public static uint jerry_create_string_from_utf8(string str)
        {
            return jerry_create_string_sz_from_utf8(
                str,
                (uint)Encoding.UTF8.GetByteCount(str)
            );
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint jerry_create_external_string_sz(
            [MarshalAs(UnmanagedType.LPUTF8Str)] string str_p,
            uint str_size,
            jerry_object_native_free_callback_t free_cb
        );

        public static uint jerry_create_external_string(string str, jerry_object_native_free_callback_t free_cb)
        {
            return jerry_create_external_string_sz(
                str,
                (uint)Encoding.UTF8.GetByteCount(str),
                free_cb
            );
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_create_symbol(uint value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(jerry_create_bigint))]
        private static extern uint jerry_create_bigint_INTERNAL(
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U8)]
            ulong[] digits,
            uint size,
            bool sign
        );

        public static uint jerry_create_bigint(ulong[] digits, bool sign)
        {
            return jerry_create_bigint_INTERNAL(digits, (uint)(sizeof(ulong) * digits.Length), sign);
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_create_undefined();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_create_realm();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint jerry_set_property(uint obj, uint property_name, uint property_value);
    }
}