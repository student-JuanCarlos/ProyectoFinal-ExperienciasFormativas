#!/bin/bash
echo "Esperando SQL Server..."
sleep 20
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P ${Password} -i /init.sql
echo "BD inicializada!"