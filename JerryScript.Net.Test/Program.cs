using System;
using static JerryScript.Net.NativeBindings.JerryScriptCore;

namespace JerryScript.Net.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var script = "print();";

            jerry_init(jerry_init_flag_t.JERRY_INIT_EMPTY);
            {
                var glob_handle = jerry_get_global_object();
                var str_handle = jerry_create_string_from_utf8("print");

                unsafe
                {
                    var func_handle = jerry_create_external_function((obj, val, p, count) =>
                    {
                        Console.WriteLine("yay :D");
                        return jerry_create_undefined();
                    });

                    var set_result = jerry_set_property(glob_handle, str_handle, func_handle);

                    jerry_release_value(set_result);
                    jerry_release_value(str_handle);
                    jerry_release_value(func_handle);
                    jerry_release_value(glob_handle);
                }
            }

            var parsed_handle = jerry_parse("unnamed", script, 0u);
            var ret_val = jerry_run(parsed_handle);
            
            jerry_release_value(ret_val);
            jerry_release_value(parsed_handle);
            jerry_cleanup();
        }
    }
}