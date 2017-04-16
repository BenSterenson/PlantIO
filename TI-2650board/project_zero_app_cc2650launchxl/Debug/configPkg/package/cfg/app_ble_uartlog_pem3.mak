#
#  Do not edit this file.  This file is generated from 
#  package.bld.  Any modifications to this file will be 
#  overwritten whenever makefiles are re-generated.
#
#  target compatibility key = ti.targets.arm.elf.M3{1,0,15.12,3
#
ifeq (,$(MK_NOGENDEPS))
-include package/cfg/app_ble_uartlog_pem3.oem3.dep
package/cfg/app_ble_uartlog_pem3.oem3.dep: ;
endif

package/cfg/app_ble_uartlog_pem3.oem3: | .interfaces
package/cfg/app_ble_uartlog_pem3.oem3: package/cfg/app_ble_uartlog_pem3.c package/cfg/app_ble_uartlog_pem3.mak
	@$(RM) $@.dep
	$(RM) $@
	@$(MSG) clem3 $< ...
	$(ti.targets.arm.elf.M3.rootDir)/bin/armcl -c  -mv7M3 --code_state=16 -me -O4 --opt_for_speed=0 --include_path="C:/Users/Michael/workspace_v6_2/project_zero_app_cc2650launchxl/Profiles" --include_path="C:/Users/Michael/workspace_v6_2/project_zero_app_cc2650launchxl/Application" --include_path="C:/Users/Michael/workspace_v6_2/project_zero_app_cc2650launchxl/ICallBLE" --include_path="C:/Users/Michael/workspace_v6_2/project_zero_app_cc2650launchxl/Startup" --include_path="C:/Users/Michael/workspace_v6_2/project_zero_app_cc2650launchxl/Board" --include_path="c:/ti/simplelink_academy_01_11_00_0000/modules/projects/support_files/Components/uart_log" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/examples/simple_peripheral/cc26xx/app" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/inc" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/icall/inc" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/profiles/roles/cc26xx" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/profiles/roles" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/profiles/dev_info" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/profiles/simple_profile/cc26xx" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/profiles/simple_profile" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/common/cc26xx" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/heapmgr" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/controller/cc26xx/inc" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/hal/src/target/_common" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/hal/src/target" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/hal/src/target/_common/cc26xx" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/hal/src/inc" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/osal/src/inc" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/services/src/sdata" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/services/src/saddr" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/icall/src/inc" --include_path="c:/ti/tirtos_cc13xx_cc26xx_2_20_01_08/products/cc26xxware_2_24_02_17393" --include_path="C:/ti/ccsv6/tools/compiler/arm_15.12.3.LTS/include" -g --c99 --gcc --define=USE_ICALL --define=POWER_SAVING --define=SBP_TASK_STACK_SIZE=700 --define=GAPROLE_TASK_STACK_SIZE=520 --define=HEAPMGR_SIZE=0 --define=Display_DISABLE_ALL --define=BOARD_DISPLAY_EXCLUDE_UART --define=xBOARD_DISPLAY_EXCLUDE_LCD --define=ICALL_MAX_NUM_TASKS=3 --define=ICALL_MAX_NUM_ENTITIES=6 --define=xdc_runtime_Assert_DISABLE_ALL --define=Xxdc_runtime_Log_DISABLE_ALL --define=MAX_NUM_BLE_CONNS=1 --define=CC26XX --define=xdc_FILE="""" --define=UARTLOG_NUM_EVT_BUF=32 --diag_warning=225 --diag_warning=255 --diag_wrap=off --display_error_number --gen_func_subsections=on --abi=eabi   -qq -pdsw225 -ms --fp_mode=strict --endian=little -mv7M3 --abi=eabi -eo.oem3 -ea.sem3   -Dxdc_cfg__xheader__='"configPkg/package/cfg/app_ble_uartlog_pem3.h"'  -Dxdc_target_name__=M3 -Dxdc_target_types__=ti/targets/arm/elf/std.h -Dxdc_bld__profile_release -Dxdc_bld__vers_1_0_15_12_3 -O2  $(XDCINCS) -I$(ti.targets.arm.elf.M3.rootDir)/include  -fs=./package/cfg -fr=./package/cfg -fc $<
	$(MKDEP) -a $@.dep -p package/cfg -s oem3 $< -C   -mv7M3 --code_state=16 -me -O4 --opt_for_speed=0 --include_path="C:/Users/Michael/workspace_v6_2/project_zero_app_cc2650launchxl/Profiles" --include_path="C:/Users/Michael/workspace_v6_2/project_zero_app_cc2650launchxl/Application" --include_path="C:/Users/Michael/workspace_v6_2/project_zero_app_cc2650launchxl/ICallBLE" --include_path="C:/Users/Michael/workspace_v6_2/project_zero_app_cc2650launchxl/Startup" --include_path="C:/Users/Michael/workspace_v6_2/project_zero_app_cc2650launchxl/Board" --include_path="c:/ti/simplelink_academy_01_11_00_0000/modules/projects/support_files/Components/uart_log" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/examples/simple_peripheral/cc26xx/app" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/inc" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/icall/inc" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/profiles/roles/cc26xx" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/profiles/roles" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/profiles/dev_info" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/profiles/simple_profile/cc26xx" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/profiles/simple_profile" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/common/cc26xx" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/heapmgr" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/controller/cc26xx/inc" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/hal/src/target/_common" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/hal/src/target" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/hal/src/target/_common/cc26xx" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/hal/src/inc" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/osal/src/inc" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/services/src/sdata" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/services/src/saddr" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/icall/src/inc" --include_path="c:/ti/tirtos_cc13xx_cc26xx_2_20_01_08/products/cc26xxware_2_24_02_17393" --include_path="C:/ti/ccsv6/tools/compiler/arm_15.12.3.LTS/include" -g --c99 --gcc --define=USE_ICALL --define=POWER_SAVING --define=SBP_TASK_STACK_SIZE=700 --define=GAPROLE_TASK_STACK_SIZE=520 --define=HEAPMGR_SIZE=0 --define=Display_DISABLE_ALL --define=BOARD_DISPLAY_EXCLUDE_UART --define=xBOARD_DISPLAY_EXCLUDE_LCD --define=ICALL_MAX_NUM_TASKS=3 --define=ICALL_MAX_NUM_ENTITIES=6 --define=xdc_runtime_Assert_DISABLE_ALL --define=Xxdc_runtime_Log_DISABLE_ALL --define=MAX_NUM_BLE_CONNS=1 --define=CC26XX --define=xdc_FILE="""" --define=UARTLOG_NUM_EVT_BUF=32 --diag_warning=225 --diag_warning=255 --diag_wrap=off --display_error_number --gen_func_subsections=on --abi=eabi   -qq -pdsw225 -ms --fp_mode=strict --endian=little -mv7M3 --abi=eabi -eo.oem3 -ea.sem3   -Dxdc_cfg__xheader__='"configPkg/package/cfg/app_ble_uartlog_pem3.h"'  -Dxdc_target_name__=M3 -Dxdc_target_types__=ti/targets/arm/elf/std.h -Dxdc_bld__profile_release -Dxdc_bld__vers_1_0_15_12_3 -O2  $(XDCINCS) -I$(ti.targets.arm.elf.M3.rootDir)/include  -fs=./package/cfg -fr=./package/cfg
	-@$(FIXDEP) $@.dep $@.dep
	
package/cfg/app_ble_uartlog_pem3.oem3: export C_DIR=
package/cfg/app_ble_uartlog_pem3.oem3: PATH:=$(ti.targets.arm.elf.M3.rootDir)/bin/;$(PATH)
package/cfg/app_ble_uartlog_pem3.oem3: Path:=$(ti.targets.arm.elf.M3.rootDir)/bin/;$(PATH)

package/cfg/app_ble_uartlog_pem3.sem3: | .interfaces
package/cfg/app_ble_uartlog_pem3.sem3: package/cfg/app_ble_uartlog_pem3.c package/cfg/app_ble_uartlog_pem3.mak
	@$(RM) $@.dep
	$(RM) $@
	@$(MSG) clem3 -n $< ...
	$(ti.targets.arm.elf.M3.rootDir)/bin/armcl -c -n -s --symdebug:none -mv7M3 --code_state=16 -me -O4 --opt_for_speed=0 --include_path="C:/Users/Michael/workspace_v6_2/project_zero_app_cc2650launchxl/Profiles" --include_path="C:/Users/Michael/workspace_v6_2/project_zero_app_cc2650launchxl/Application" --include_path="C:/Users/Michael/workspace_v6_2/project_zero_app_cc2650launchxl/ICallBLE" --include_path="C:/Users/Michael/workspace_v6_2/project_zero_app_cc2650launchxl/Startup" --include_path="C:/Users/Michael/workspace_v6_2/project_zero_app_cc2650launchxl/Board" --include_path="c:/ti/simplelink_academy_01_11_00_0000/modules/projects/support_files/Components/uart_log" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/examples/simple_peripheral/cc26xx/app" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/inc" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/icall/inc" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/profiles/roles/cc26xx" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/profiles/roles" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/profiles/dev_info" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/profiles/simple_profile/cc26xx" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/profiles/simple_profile" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/common/cc26xx" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/heapmgr" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/controller/cc26xx/inc" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/hal/src/target/_common" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/hal/src/target" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/hal/src/target/_common/cc26xx" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/hal/src/inc" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/osal/src/inc" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/services/src/sdata" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/services/src/saddr" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/icall/src/inc" --include_path="c:/ti/tirtos_cc13xx_cc26xx_2_20_01_08/products/cc26xxware_2_24_02_17393" --include_path="C:/ti/ccsv6/tools/compiler/arm_15.12.3.LTS/include" -g --c99 --gcc --define=USE_ICALL --define=POWER_SAVING --define=SBP_TASK_STACK_SIZE=700 --define=GAPROLE_TASK_STACK_SIZE=520 --define=HEAPMGR_SIZE=0 --define=Display_DISABLE_ALL --define=BOARD_DISPLAY_EXCLUDE_UART --define=xBOARD_DISPLAY_EXCLUDE_LCD --define=ICALL_MAX_NUM_TASKS=3 --define=ICALL_MAX_NUM_ENTITIES=6 --define=xdc_runtime_Assert_DISABLE_ALL --define=Xxdc_runtime_Log_DISABLE_ALL --define=MAX_NUM_BLE_CONNS=1 --define=CC26XX --define=xdc_FILE="""" --define=UARTLOG_NUM_EVT_BUF=32 --diag_warning=225 --diag_warning=255 --diag_wrap=off --display_error_number --gen_func_subsections=on --abi=eabi   -qq -pdsw225 --endian=little -mv7M3 --abi=eabi -eo.oem3 -ea.sem3   -Dxdc_cfg__xheader__='"configPkg/package/cfg/app_ble_uartlog_pem3.h"'  -Dxdc_target_name__=M3 -Dxdc_target_types__=ti/targets/arm/elf/std.h -Dxdc_bld__profile_release -Dxdc_bld__vers_1_0_15_12_3 -O2  $(XDCINCS) -I$(ti.targets.arm.elf.M3.rootDir)/include  -fs=./package/cfg -fr=./package/cfg -fc $<
	$(MKDEP) -a $@.dep -p package/cfg -s oem3 $< -C  -n -s --symdebug:none -mv7M3 --code_state=16 -me -O4 --opt_for_speed=0 --include_path="C:/Users/Michael/workspace_v6_2/project_zero_app_cc2650launchxl/Profiles" --include_path="C:/Users/Michael/workspace_v6_2/project_zero_app_cc2650launchxl/Application" --include_path="C:/Users/Michael/workspace_v6_2/project_zero_app_cc2650launchxl/ICallBLE" --include_path="C:/Users/Michael/workspace_v6_2/project_zero_app_cc2650launchxl/Startup" --include_path="C:/Users/Michael/workspace_v6_2/project_zero_app_cc2650launchxl/Board" --include_path="c:/ti/simplelink_academy_01_11_00_0000/modules/projects/support_files/Components/uart_log" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/examples/simple_peripheral/cc26xx/app" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/inc" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/icall/inc" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/profiles/roles/cc26xx" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/profiles/roles" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/profiles/dev_info" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/profiles/simple_profile/cc26xx" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/profiles/simple_profile" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/common/cc26xx" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/heapmgr" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/controller/cc26xx/inc" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/hal/src/target/_common" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/hal/src/target" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/hal/src/target/_common/cc26xx" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/hal/src/inc" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/osal/src/inc" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/services/src/sdata" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/services/src/saddr" --include_path="c:/ti/simplelink/ble_sdk_2_02_01_18/src/components/icall/src/inc" --include_path="c:/ti/tirtos_cc13xx_cc26xx_2_20_01_08/products/cc26xxware_2_24_02_17393" --include_path="C:/ti/ccsv6/tools/compiler/arm_15.12.3.LTS/include" -g --c99 --gcc --define=USE_ICALL --define=POWER_SAVING --define=SBP_TASK_STACK_SIZE=700 --define=GAPROLE_TASK_STACK_SIZE=520 --define=HEAPMGR_SIZE=0 --define=Display_DISABLE_ALL --define=BOARD_DISPLAY_EXCLUDE_UART --define=xBOARD_DISPLAY_EXCLUDE_LCD --define=ICALL_MAX_NUM_TASKS=3 --define=ICALL_MAX_NUM_ENTITIES=6 --define=xdc_runtime_Assert_DISABLE_ALL --define=Xxdc_runtime_Log_DISABLE_ALL --define=MAX_NUM_BLE_CONNS=1 --define=CC26XX --define=xdc_FILE="""" --define=UARTLOG_NUM_EVT_BUF=32 --diag_warning=225 --diag_warning=255 --diag_wrap=off --display_error_number --gen_func_subsections=on --abi=eabi   -qq -pdsw225 --endian=little -mv7M3 --abi=eabi -eo.oem3 -ea.sem3   -Dxdc_cfg__xheader__='"configPkg/package/cfg/app_ble_uartlog_pem3.h"'  -Dxdc_target_name__=M3 -Dxdc_target_types__=ti/targets/arm/elf/std.h -Dxdc_bld__profile_release -Dxdc_bld__vers_1_0_15_12_3 -O2  $(XDCINCS) -I$(ti.targets.arm.elf.M3.rootDir)/include  -fs=./package/cfg -fr=./package/cfg
	-@$(FIXDEP) $@.dep $@.dep
	
package/cfg/app_ble_uartlog_pem3.sem3: export C_DIR=
package/cfg/app_ble_uartlog_pem3.sem3: PATH:=$(ti.targets.arm.elf.M3.rootDir)/bin/;$(PATH)
package/cfg/app_ble_uartlog_pem3.sem3: Path:=$(ti.targets.arm.elf.M3.rootDir)/bin/;$(PATH)

clean,em3 ::
	-$(RM) package/cfg/app_ble_uartlog_pem3.oem3
	-$(RM) package/cfg/app_ble_uartlog_pem3.sem3

app_ble_uartlog.pem3: package/cfg/app_ble_uartlog_pem3.oem3 package/cfg/app_ble_uartlog_pem3.mak

clean::
	-$(RM) package/cfg/app_ble_uartlog_pem3.mak
