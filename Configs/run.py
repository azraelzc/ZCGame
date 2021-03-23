import os
import xlrd
import shutil
import Excel2Lua



if __name__ == '__main__':
    print("excel2lua start")
    test_excel_path = os.path.abspath('excel')
    test_output_path = os.path.abspath('configs')
    output_list = Excel2Lua.do_convert(test_excel_path, test_output_path)
    
    print(output_list)
    copy_dir = os.path.abspath('../Client/LuaProject/configs')
    isExists = os.path.exists(copy_dir)
    if isExists:
        shutil.rmtree(copy_dir)
    shutil.copytree(test_output_path,copy_dir)
        



