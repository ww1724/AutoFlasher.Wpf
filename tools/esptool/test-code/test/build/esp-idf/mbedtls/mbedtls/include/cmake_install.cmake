# Install script for directory: T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include

# Set the install prefix
if(NOT DEFINED CMAKE_INSTALL_PREFIX)
  set(CMAKE_INSTALL_PREFIX "C:/Program Files (x86)/test")
endif()
string(REGEX REPLACE "/$" "" CMAKE_INSTALL_PREFIX "${CMAKE_INSTALL_PREFIX}")

# Set the install configuration name.
if(NOT DEFINED CMAKE_INSTALL_CONFIG_NAME)
  if(BUILD_TYPE)
    string(REGEX REPLACE "^[^A-Za-z0-9_]+" ""
           CMAKE_INSTALL_CONFIG_NAME "${BUILD_TYPE}")
  else()
    set(CMAKE_INSTALL_CONFIG_NAME "")
  endif()
  message(STATUS "Install configuration: \"${CMAKE_INSTALL_CONFIG_NAME}\"")
endif()

# Set the component getting installed.
if(NOT CMAKE_INSTALL_COMPONENT)
  if(COMPONENT)
    message(STATUS "Install component: \"${COMPONENT}\"")
    set(CMAKE_INSTALL_COMPONENT "${COMPONENT}")
  else()
    set(CMAKE_INSTALL_COMPONENT)
  endif()
endif()

# Is this installation the result of a crosscompile?
if(NOT DEFINED CMAKE_CROSSCOMPILING)
  set(CMAKE_CROSSCOMPILING "TRUE")
endif()

# Set default install directory permissions.
if(NOT DEFINED CMAKE_OBJDUMP)
  set(CMAKE_OBJDUMP "T:/Develop/Expressif/tools/riscv32-esp-elf/esp-2022r1-11.2.0/riscv32-esp-elf/bin/riscv32-esp-elf-objdump.exe")
endif()

if(CMAKE_INSTALL_COMPONENT STREQUAL "Unspecified" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/mbedtls" TYPE FILE PERMISSIONS OWNER_READ OWNER_WRITE GROUP_READ WORLD_READ FILES
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/aes.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/aria.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/asn1.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/asn1write.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/base64.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/bignum.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/build_info.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/camellia.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/ccm.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/chacha20.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/chachapoly.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/check_config.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/cipher.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/cmac.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/compat-2.x.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/config_psa.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/constant_time.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/ctr_drbg.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/debug.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/des.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/dhm.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/ecdh.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/ecdsa.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/ecjpake.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/ecp.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/entropy.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/error.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/gcm.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/hkdf.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/hmac_drbg.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/legacy_or_psa.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/lms.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/mbedtls_config.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/md.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/md5.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/memory_buffer_alloc.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/net_sockets.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/nist_kw.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/oid.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/pem.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/pk.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/pkcs12.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/pkcs5.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/pkcs7.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/platform.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/platform_time.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/platform_util.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/poly1305.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/private_access.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/psa_util.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/ripemd160.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/rsa.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/sha1.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/sha256.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/sha512.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/ssl.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/ssl_cache.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/ssl_ciphersuites.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/ssl_cookie.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/ssl_ticket.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/threading.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/timing.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/version.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/x509.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/x509_crl.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/x509_crt.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/mbedtls/x509_csr.h"
    )
endif()

if(CMAKE_INSTALL_COMPONENT STREQUAL "Unspecified" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/psa" TYPE FILE PERMISSIONS OWNER_READ OWNER_WRITE GROUP_READ WORLD_READ FILES
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/psa/crypto.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/psa/crypto_builtin_composites.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/psa/crypto_builtin_primitives.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/psa/crypto_compat.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/psa/crypto_config.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/psa/crypto_driver_common.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/psa/crypto_driver_contexts_composites.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/psa/crypto_driver_contexts_primitives.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/psa/crypto_extra.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/psa/crypto_platform.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/psa/crypto_se_driver.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/psa/crypto_sizes.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/psa/crypto_struct.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/psa/crypto_types.h"
    "T:/Develop/Expressif/esp-idf/components/mbedtls/mbedtls/include/psa/crypto_values.h"
    )
endif()

