#!/bin/bash

OUTPUT_FILE="new_passwords.txt"
> $OUTPUT_FILE

for username in "$@"
do
    password=$(openssl rand -base64 9 | cut -c1-12)
    echo "$username:$password" >> $OUTPUT_FILE
done