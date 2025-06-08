# JSONConverter
Тестовое задание на стажировку в компанию "Цифра" по направлению "Разработка бэкенд (C#)".

## Изменение в структуре данных
Был добавлен подраздел "content" в выходной файл JSON для случаев, когда в исходном файле у раздела есть как содержимое, так и подразделы.

### Пример 
*Входной файл:*  
1 Scope  
Part 1.  
2 Reference  
documents  
3 Terms, definitions, and abbreviations  
purposes.  
3.1 OPC UA terms  
3.1.1 AddressSpace  
collection.

*Выходной файл:*  
```json
{
  "1 Scope": "Part 1.",
  "2 Reference ": "documents",
  "3 Terms, definitions, and abbreviations": {
    "content": "purposes.",
    "3.1 OPC UA terms": {
      "3.1.1 AddressSpace": "collection."
    }
  }
}
```
