# Distributed under the OSI-approved BSD 3-Clause License.  See accompanying
# file Copyright.txt or https://cmake.org/licensing for details.

cmake_minimum_required(VERSION 3.5)

file(MAKE_DIRECTORY
  "T:/Develop/Expressif/esp-idf/components/bootloader/subproject"
  "T:/Document/python/esptool/test-code/vertify/build/bootloader"
  "T:/Document/python/esptool/test-code/vertify/build/bootloader-prefix"
  "T:/Document/python/esptool/test-code/vertify/build/bootloader-prefix/tmp"
  "T:/Document/python/esptool/test-code/vertify/build/bootloader-prefix/src/bootloader-stamp"
  "T:/Document/python/esptool/test-code/vertify/build/bootloader-prefix/src"
  "T:/Document/python/esptool/test-code/vertify/build/bootloader-prefix/src/bootloader-stamp"
)

set(configSubDirs )
foreach(subDir IN LISTS configSubDirs)
    file(MAKE_DIRECTORY "T:/Document/python/esptool/test-code/vertify/build/bootloader-prefix/src/bootloader-stamp/${subDir}")
endforeach()
if(cfgdir)
  file(MAKE_DIRECTORY "T:/Document/python/esptool/test-code/vertify/build/bootloader-prefix/src/bootloader-stamp${cfgdir}") # cfgdir has leading slash
endif()
