rm -f -r /usr/bin/SIL/PublishingSolution/*
mkdir -p /usr/bin/SIL/PublishingSolution
cp -r . /usr/bin/SIL/PublishingSolution
echo mono /usr/bin/SIL/PublishingSolution/PublishingSolution.exe >/usr/bin/PublishingSolution
chmod 555 /usr/bin/PublishingSolution
rm -f -r /usr/share/SIL/PublishingSolutions/*
mkdir -p /usr/share/SIL/PublishingSolutions
chmod 777 /usr/share/SIL/PublishingSolutions

