using ProjectGamma_0.models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using System.IO;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;

namespace ProjectGamma_0.controllers
{
	public class DBController :PersonUserBaseDate
	{
		PasswordHasher passwordHasher = new PasswordHasher();
	
		public DBController(string? name, string? job, string? login, string? password) // ����������� � ������� ��������
		{                                                                                   //�� �  �������� ������ � �������														//Id � �����
			Id = Guid.NewGuid();
			this.name = name;
			this.job = job;
			this.login = login;
			this.password = passwordHasher?.Generate(password!); //���������� ����������
			this.link = $"Users/id{Id}/index.html";
        }

    }
	}
