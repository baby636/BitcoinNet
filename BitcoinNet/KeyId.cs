﻿using BitcoinNet.Crypto;
using BitcoinNet.DataEncoders;
using System;

namespace BitcoinNet
{
	public abstract class TxDestination : IDestination
	{
		internal byte[] _DestBytes;

		public TxDestination()
		{
			_DestBytes = new byte[] { 0 };
		}

		public TxDestination(byte[] value)
		{
			if(value == null)
				throw new ArgumentNullException(nameof(value));
			_DestBytes = value;
		}

		public TxDestination(string value)
		{
			_DestBytes = Encoders.Hex.DecodeData(value);
			_Str = value;
		}

		public abstract BitcoinAddress GetAddress(Network network);

		// IDestination Members

		public abstract Script ScriptPubKey
		{
			get;
		}

		public byte[] ToBytes()
		{
			return ToBytes(false);
		}
		public byte[] ToBytes(bool @unsafe)
		{
			if(@unsafe)
				return _DestBytes;
			var array = new byte[_DestBytes.Length];
			Array.Copy(_DestBytes, array, _DestBytes.Length);
			return array;
		}

		public override bool Equals(object obj)
		{
			TxDestination item = obj as TxDestination;
			if(item == null)
				return false;
			return Utils.ArrayEqual(_DestBytes, item._DestBytes) && item.GetType() == this.GetType();
		}
		public static bool operator ==(TxDestination a, TxDestination b)
		{
			if(System.Object.ReferenceEquals(a, b))
				return true;
			if(((object)a == null) || ((object)b == null))
				return false;
			return Utils.ArrayEqual(a._DestBytes, b._DestBytes) && a.GetType() == b.GetType();
		}

		public static bool operator !=(TxDestination a, TxDestination b)
		{
			return !(a == b);
		}

		public override int GetHashCode()
		{
			return Utils.GetHashCode(_DestBytes);
		}

		string _Str;
		public override string ToString()
		{
			if(_Str == null)
				_Str = Encoders.Hex.EncodeData(_DestBytes);
			return _Str;
		}
	}
	public class KeyId : TxDestination
	{
		public KeyId()
			: this(0)
		{

		}

		public KeyId(byte[] value)
			: base(value)
		{
			if(value.Length != 20)
				throw new ArgumentException("value should be 20 bytes", "value");
		}
		public KeyId(uint160 value)
			: base(value.ToBytes())
		{

		}

		public KeyId(string value)
			: base(value)
		{
		}

		public override Script ScriptPubKey
		{
			get
			{
				return PayToPubkeyHashTemplate.Instance.GenerateScriptPubKey(this);
			}
		}

		public override BitcoinAddress GetAddress(Network network)
		{
			return new BitcoinPubKeyAddress(this, network);
		}
	}
	public class ScriptId : TxDestination
	{
		public ScriptId()
			: this(0)
		{

		}

		public ScriptId(byte[] value)
			: base(value)
		{
			if(value.Length != 20)
				throw new ArgumentException("value should be 20 bytes", "value");
		}
		public ScriptId(uint160 value)
			: base(value.ToBytes())
		{

		}

		public ScriptId(string value)
			: base(value)
		{
		}

		public ScriptId(Script script)
			: this(Hashes.Hash160(script._Script))
		{
		}

		public override Script ScriptPubKey
		{
			get
			{
				return PayToScriptHashTemplate.Instance.GenerateScriptPubKey(this);
			}
		}

		public override BitcoinAddress GetAddress(Network network)
		{
			return new BitcoinScriptAddress(this, network);
		}
	}
}