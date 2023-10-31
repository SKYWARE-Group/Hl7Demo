# Hl7Demo

This app is a simple demo for parsing HL7 v2.5 messages ([OML_O21](https://hl7-definition.caristix.com/v2/HL7v2.5/TriggerEvents/OML_O21)) with [nHapi parser](https://github.com/nHapiNET/nHapi).

Demo scenario includes two messages:

* Fisrt message is for placing new order (examinations: Creatinine, Cholesterol HDL, Triglycerides, AST and ALT, [LOINC](https://loinc.org) coded)
* Second message cancels one of the examinations (Creatinine) from previously placed order (first message)

As a result, the final order have to contain only Cholesterol HDL, Triglycerides, AST and ALT. Alternate coding system is according to [Bulgarian NHIF](https://www.nhif.bg/).

![image](https://github.com/SKYWARE-Group/Hl7Demo/assets/10154711/7d4bb5a8-c8e5-4a50-adef-ae8d30338907)
