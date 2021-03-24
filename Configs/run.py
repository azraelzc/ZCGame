
import os
import xlrd
import shutil
import Excel2Lua



if __name__ == '__main__':
    print("excel2lua start")
    base_path = 'gen'
    test_excel_path = os.path.abspath('excel')
    test_output_path = os.path.abspath(base_path+'/configs')
    path_map = Excel2Lua.do_convert(test_excel_path, test_output_path)
    
    print(path_map)
    #创建生成配置的清单
    list_path = base_path+'/configList.lua'
    isExists = os.path.exists(list_path)
    if isExists:
        os.remove(list_path)
    list_str = 'return {'
    for file_name in path_map:
        list_str = list_str + '"' + file_name + '",'
        
    list_str = list_str + '}'
    file_writer = open(os.path.abspath(list_path), 'w', encoding='utf-8')
    file_writer.write(list_str)   
    file_writer.close()
    
    from_dir = os.path.abspath(base_path)
    copy_dir = os.path.abspath('../Client/LuaProject/gen')
    isExists = os.path.exists(copy_dir)
    if isExists:
        shutil.rmtree(copy_dir)
    shutil.copytree(from_dir,copy_dir)
        



