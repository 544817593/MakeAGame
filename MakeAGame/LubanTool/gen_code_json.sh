#!/bin/zsh
WORKSPACE=..

GEN_CLIENT=${WORKSPACE}/LubanTool/Luban.ClientServer/Luban.ClientServer.dll
CONF_ROOT=${WORKSPACE}/LubanTool/GameConfig


dotnet ${GEN_CLIENT} -j cfg --\
 -d ${CONF_ROOT}/Defines/__root__.xml \
 --input_data_dir ${CONF_ROOT}/Datas \
 --output_code_dir ${WORKSPACE}/Assets/Scripts/LubanGen \
 --output_data_dir ${WORKSPACE}/Assets/Resources/Json \
 --gen_types code_cs_unity_json,data_json \
 -s all 