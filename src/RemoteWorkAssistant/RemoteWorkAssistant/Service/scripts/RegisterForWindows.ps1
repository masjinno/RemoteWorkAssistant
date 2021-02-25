${EXE PATH} = "E:\Documents\Programs\RemoteWorkAssistant\src\RemoteWorkAssistant\RemoteWorkAssistant\Service\bin\Debug\net5.0\"
${EXE FILE PATH} = "E:\Documents\Programs\RemoteWorkAssistant\src\RemoteWorkAssistant\RemoteWorkAssistant\Service\bin\Debug\net5.0\RemoteWorkAssistant.Service.exe"
${DOMAIN OR COMPUTER NAME\USER} = "DESKTOP-HUTAFHB\ServiceUser"
${SERVICE NAME} = "RemoteWorkAssistantService"
${DESCRIPTION} = "リモートワークアシスタントのサービス"
${DISPLAY NAME} = "Remote Work Assistant Service"

echo ${BUILD CONSTITUTION}
echo ${EXE PATH}
echo ${EXE FILE PATH}
echo ${DOMAIN OR COMPUTER NAME\USER}
echo ${SERVICE NAME}
echo ${DESCRIPTION}
echo ${DISPLAY NAME}

$acl = Get-Acl "${EXE PATH}"
$aclRuleArgs = "${DOMAIN OR COMPUTER NAME\USER}", "Read,Write,ReadAndExecute", "ContainerInherit,ObjectInherit", "None", "Allow"
$accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule($aclRuleArgs)
echo $accessRule
$acl.SetAccessRule($accessRule)
$acl | Set-Acl "${EXE PATH}"

New-Service -Name ${SERVICE NAME} -BinaryPathName "${EXE FILE PATH}" -Credential "${DOMAIN OR COMPUTER NAME\USER}" -Description "${DESCRIPTION}" -DisplayName "${DISPLAY NAME}" -StartupType Automatic
